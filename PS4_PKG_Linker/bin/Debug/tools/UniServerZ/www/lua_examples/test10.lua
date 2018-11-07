--Obatain script path and added to package path
path = debug.getinfo(1,"S").source:match[[^@?(.*[\/])[^\/]-$]] 
package.path = path .."?.lua;".. package.path

require"lfs" --  LuaFileSystem

function handle(r)
   r.content_type = "text/html"

 --E) Retrieve all files: Target .html files
 -- Convert to links

 r:puts ("<br /><b>E) Target .html files. Converted to links</b><br />")

  r:puts ("<ul>") -- Start of list
  folderpath = path                           -- script
    for file in lfs.dir(folderpath) do        -- Iterate
     if file:match("%.html$") then            -- contains .lua
       r:puts ("<li><a href=\"" .. file .. "\">" .. file .. "</a></li>")
     end
  end
  r:puts ("</ul>") -- End list

end
