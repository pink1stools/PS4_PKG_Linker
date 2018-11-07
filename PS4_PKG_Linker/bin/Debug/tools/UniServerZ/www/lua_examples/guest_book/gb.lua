-- Full path to database
db_file = debug.getinfo(1,"S").source:match[[^@?(.*[\/])[^\/]-$]] .. 'gb_db.sqlite' -- database

-- Check a file exists
function file_exists(file_name)            -- Function: Check file exists
  local file_found=io.open(file_name, "r") -- Note: Full path. With forward slashes     
  if file_found==nil then
    return false -- File Not Found
  else
    io.close(file_found)
    return true -- File Found
  end
end

-- Create database if it does not exist
-- This will be run once only if the db does not exists 
function db_init(db_file) 
 if not file_exists(db_file) then           -- DB does not exist create it
  local luasql = require "luasql.sqlite3"  -- load driver
  local env = luasql.sqlite3()             -- create environment object
  local conn = env:connect(db_file)        -- connect/create to data source
  assert(conn:execute("create table if not exists guestbook(id INTEGER PRIMARY KEY, name TEXT NOT NULL, tim TEXT DEFAULT CURRENT_TIMESTAMP, msg TEXT NOT NULL)"))
  conn:close() -- close everything
  env:close()
 end
end

-- Insert into database.
function db_add_data(name,message) 
 if file_exists(db_file) then              -- DB exists add data
  local luasql = require "luasql.sqlite3"  -- load driver
  local env = luasql.sqlite3()             -- create environment object
  local conn = env:connect(db_file)        -- connect/create to data source
  assert(conn:execute("insert into guestbook (name,msg) values(" .. "'" .. name .. "'" .. "," .. "'" .. message .. "'" .. ")"))
  conn:close() -- close everything
  env:close()
 end
end

-- Get number of entries in database
function db_get_entries()
 local entries = 0  -- set initial value
 if file_exists(db_file) then              -- Assumes guestbook DB exists
  local luasql = require "luasql.sqlite3"  -- load driver
  local env = luasql.sqlite3()             -- create environment object
  local conn = env:connect(db_file)        -- connect to data source

  local cursor = conn:execute "SELECT id FROM guestbook"
  local row = cursor:fetch ({}, "a")
  while row do
   entries = entries + 1
   row = cursor:fetch (row, "a")
  end

  cursor:close() -- close everything
  conn:close()
  env:close()
 end
 return entries -- Return total number of entries
end


function split(s, delimiter) -- Split string at delimiter return table of strings
    result = {};
    for match in (s..delimiter):gmatch("(.-)"..delimiter) do
        table.insert(result, match);
    end
    return result;
end

-- Remove html code
function clean_html(str)            
  local t = str          -- Set local var
  local cleaner = {      -- list of strings to replace (the order is important to avoid conflicts)
  { "<!--", "" },        -- Comment
  { "&amp;", "" },       -- decode ampersands
  { "&#151;", "" },      -- em dash
  { "&#146;", "" },      -- right single quote
  { "&#147;", "" },      -- left double quote
  { "&#148;", "" },      -- right double quote
  { "&#150;", "" },      -- en dash
  { "&#160;", "" },      -- non-breaking space
  { "<br ?/?>", "" },    -- all <br> tags whether terminated or not (<br> <br/> <br />) become new lines
  { "</p>", "" },        -- ends of paragraphs become new lines
  { "(%b<>)", "" },      -- all other html elements are completely removed (must be done last)
  { "\r", "\n" },        -- return carriage become new lines
  { "[\n\n]+", "\n" },   -- reduce all multiple new lines with a single new line
  { "[\n]+", "<br />" }, -- reduce all multiple new lines with a single new line
  { "^\n*", "" },        -- trim new lines from the start...
  { "\n*$", "" },        -- ... and end
  }
   
  for i=1, #cleaner do           -- clean html from the string
   local cleans = cleaner[i]
   t = string.gsub( t, cleans[1], cleans[2] )
  end
 return t -- Return cleaned string
end 



-- ##### Main Function #########
function handle(r)

 db_init(db_file)           -- Create database if it does not exist

 r.content_type = "text/html"

r:puts [[
 <html>
  <head><title>Guest book</title></head>
  <link rel="stylesheet" type="text/css" href="style.css">
  <body>
  <div id="main">
]]

-- Get form data 
   -- Set initial values
   name        = ''     -- Set initial values
   msg         = ''     -- Assumes form was initial request
   add_comment = false  -- Button pressed true default false
   up          = false  -- Button pressed true default false
   down        = false  -- Button pressed true default false
   pointer     = 0      -- Current database location

   for k, v in pairs(r:parsebody()) do -- Read data from form body.
      if k=='comment_f' then
         msg=v                 -- message
      elseif k=='name_f' then
         name=v                -- user name
      elseif k=='add_comment_f' then
         add_comment=true      -- button pressed
      elseif k=='up_f' then
         up=true               -- button pressed
      elseif k=='down_f' then
         down=true             -- button pressed
      elseif k=='pointer_f' then
         pointer=v             -- Hidden pointer number
      end
   end

   name = name:gsub('[^A-Za-z0-9 ]','')  -- Clean name
   msg  = msg:gsub('[^A-Za-z0-9\n ]','') -- Clean message
   msg_arry = split(msg, '\n') -- Split string on \n

  --Limit name and msg line lengths
  name_length = 30 -- Mamimum number of characters
  msg_length  = 80 -- Mamimum number of characters

  if #name >= name_length then              -- Check name length
    name = string.sub(name, 1,name_length)  -- Extract shorter length
  end

  for i=1, #msg_arry do                     -- Scan message array
   if #msg_arry[i] >= msg_length then       -- Check msg line length
     msg_arry[i] = string.sub(msg_arry[i], 1,msg_length)
   end
  end  


  -- Limit number of lines
  lines = 3 -- Mamimum number of lines
  msg_lines = ''
  max = 0
  if #msg_arry <= lines then
    max = #msg_arry
  else
    max = lines
  end

  for i=1, max do    -- Build string limited to max lines
   msg_lines = msg_lines .. msg_arry[i] .. '\n'
  end
  -- END Limit lines

   msg = clean_html(msg_lines) -- Clean and replace /n with <br />


   -- Set database pointer initial value. Reads data from this lcation
   if (not add_comment and not up and not down) then
     pointer = db_get_entries() -- Set current value
   end

   -- Adjust pointer value depending on up or down button
   if up then                -- Up button clicked
    pointer = pointer + 3    -- Increment
    max = db_get_entries()
    if pointer >= max then   -- Cannot be greater
      pointer = max          -- Hence set max
    end
   end

   if down then              -- Down button clicked
    pointer = pointer - 3    -- Decrement
    if pointer <= 1 then     -- Cannot be negative
      pointer = 3            -- Hence set it to zero
    end
   end

-- Add content input from form
 if add_comment then
   db_add_data(name,msg)      -- Insert data into database
   pointer = db_get_entries() -- Get new entries count
 end


-- Display gestbook content
 if file_exists(db_file) then              -- Assumes guestbook DB exists
  local luasql = require "luasql.sqlite3"  -- load driver
  local env = luasql.sqlite3()             -- create environment object
  local conn = env:connect(db_file)        -- connect to data source

  local cursor = conn:execute ("SELECT id, name, tim, msg FROM guestbook WHERE id <= " .. "'" .. pointer .. "' ORDER BY id DESC LIMIT 3 ")

  local row = cursor:fetch ({}, "a")

  while row do

   r:puts("\n<fieldset>")
   r:puts("\n<legend>" .. row.id .. "</legend>")
   r:puts("\n<span class='lin1'>Posted by:</span><span class='lin2'> " .. row.name .. "</span><span class='lin3'> On: </span><span class='lin4'>" .. row.tim .. "</span><br />")
   r:puts(row.msg)
   r:puts("\n</fieldset>")

    row = cursor:fetch (row, "a")
  end


  cursor:close() -- close everything
  conn:close()
  env:close()
 end
-- END Display gestbook content


-- Display form
r:puts [[

<h2>Add a comment!</h2>
    <form method="post" action="">
        <textarea name="comment_f" rows="5" style="width:100%"></textarea><br />
        Posted By: <input type="text" name="name_f">
        <input class="submit" type="submit" name="add_comment_f" value="Add Comment">
        &nbsp;&nbsp;Scroll entries: 
        <input class="submit" type="submit" name="up_f" value="Up">
        <input class="submit" type="submit" name="down_f" value="Down">
]]

r:puts("<input type='hidden' name='pointer_f' value='" .. pointer .. "' />")

r:puts [[
    </form>
]]


r:puts [[
</div>  </body></html>
]]


end
