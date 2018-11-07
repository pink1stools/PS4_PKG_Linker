--Obatain script path and added to package path
path = debug.getinfo(1,"S").source:match[[^@?(.*[\/])[^\/]-$]] 
package.path = path .."?.lua;".. package.path

require"lfs" --  LuaFileSystem

function handle(r)
 r.content_type = "text/html" -- Required first line


 --A) Retrieve all files and display result
 r:puts ("<b>A) Retrieve all files</b><br />")
 folderpath = path                           -- script
   for filename in lfs.dir(folderpath) do    -- Iterate
        r:puts (filename .. "<br />")        -- output
   end

 --B) Retrieve all files remove . and ..
 r:puts ("<br /><b>B) All files: Removed . and ..</b><br />")
 folderpath = path                           -- script
   for file in lfs.dir(folderpath) do        -- Iterate
      if file ~= "." and file ~= ".." then   -- remove . ..
        r:puts (file .. "<br />")            -- output
      end
   end

 --C) Retrieve all files display only folders
 r:puts ("<br /><b>C) Display only folders</b><br />")
 folderpath = path                           -- script
   for file in lfs.dir(folderpath) do        -- Iterate
     if not file:match("%.") then   -- "%." an escaped "."
        r:puts (file .. "<br />")
     end
   end


 --D) Retrieve all files: Display only files
 r:puts ("<br /><b>D) Display only files</b><br />")
 folderpath = path                           -- script
   for file in lfs.dir(folderpath) do        -- Iterate
      if file ~= "." and file ~= ".." then   -- remove . ..
         if file:match("%.") then            -- contains .
           r:puts (file .. "<br />")
         end
      end
   end


 --E) Retrieve all files: Target .html files
 r:puts ("<br /><b>E) Target .html files</b><br />")

 folderpath = path                           -- script
   for file in lfs.dir(folderpath) do        -- Iterate
     if file:match("%.html$") then           -- contains .html
       -- "%." is an escaped ".", "$" is end of string
       r:puts (file .. "<br />")
     end
  end


 --F) Retrieve all files: Target specific file
 r:puts ("<br /><b>F) Target specific</b><br />")

 specific = "index.lua"                 -- Target
 found = false  -- Set flag to false
 folderpath = path                      -- script
   for file in lfs.dir(folderpath) do   -- Iterate
     if file:match("^index%.lua$") then -- is the file
       found = true                     -- Found set flag
     end
   end

 if found then 
   r:puts ("File " .. specific .. "  Exists<br />")
 else
   r:puts ("File " .. specific .. "  Does not Exist<br />") 
 end


 --G) Use escape: Target specific file
 r:puts ("<br /><b>F) Use escape: Target specific file</b><br />")

 specific    = "index.lua"                 -- Target
 newspecific = string.gsub(specific, "%p", "%%%1") -- general escape


 found = false  -- Set flag to false
 folderpath = path                      -- script
   for file in lfs.dir(folderpath) do   -- Iterate
     if file:match("^" .. newspecific .."$") then -- is the file
       found = true                     -- Found set flag
     end
   end

 if found then 
   r:puts ("File " .. specific .. "  Exists<br />")
 else
   r:puts ("File " .. specific .. "  Does not Exist<br />") 
 end


end
