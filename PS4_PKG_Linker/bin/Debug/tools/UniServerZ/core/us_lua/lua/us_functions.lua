-- #############################################################
-- # Name: us_functions.lua
-- # Created By: The Uniform Server Development Team
-- # Edited Last By: Mike Gleaves (ric)
-- # V 1.0 19-5-2013
-- ##############################################################



-- === Check file exists ====
-- file is the full path
function file_exists(file)

 local ret = false          -- Assume file not found

 -- Split file into sections
 a,b,c = string.match(file, "(.-)([^/]-([^%.]+))$")
 local folderpath = a  -- e.g C:/UniServerZ/www/templates/
 local specific   = b  -- e.g mpg.lua Target
 local extension  = c  -- e.g lua 

 -- Escape the special regex characters
 local newspecific = string.gsub(specific, "%p", "%%%1")

 -- Iterate files in specified folder
 for file in lfs.dir(folderpath) do             -- Iterate
   if file:match("^" .. newspecific .."$") then -- found
     ret = true                                 -- set flag
   end
 end

 return ret
end
-- === End file_exists(file)

-- Check requested page is valid
function valid_page(r)
  local ret = true -- Assume valid
  if r.method == 'GET' then             -- 1)Check method
   if r:parseargs().action then         -- 2)Arg exists
    local action = r:parseargs().action -- 3)Get value
     if action ~="" then                -- 4)Is action set
       -- 5) Sanitise
       local patt = "^[%w_-]+$"         -- Set pattern
       if string.match (action,patt) then

         -- 6)Exclude header and footer content
         if action=='header' or action=='footer'  then
          ret = false
         else

           -- 7) Check file exists
           local f = path..'lua_templates/'..action..'.lua'
           if file_exists(f) then
            ret = true   -- File exists
           else
            ret = false  -- File does not exist
           end
         end

       else            -- No patern match
         ret = false   -- Text invalid
       end
     else              -- Action blank
       ret = false
     end
   else                -- No Action arg
    ret = false
   end
  else                 -- Not Get method         
   ret = false 
  end
  return ret
end
-- End valid_page(r)