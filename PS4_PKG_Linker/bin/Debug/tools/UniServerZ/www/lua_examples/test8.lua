function handle(r)
   r.content_type = "text/html"

directory = debug.getinfo(1,"S").source:match[[^@?(.*[\/])[^\/]-$]]
-- e.g directory = "C:/UniServerZ/www/lua_examples/"

r:puts ("=== Folders and files === <br />")
for dir in io.popen("dir \"" .. directory ..  "\"/b"):lines() do r:puts(dir .."<br />") end

r:puts ("=== Folders === <br />")
for dir in io.popen("dir \"" .. directory ..  "\"/ad /b"):lines() do r:puts(dir .."<br />") end

r:puts ("=== Files === <br />")
for dir in io.popen("dir \"" .. directory ..  "\"/a:-d /b"):lines() do r:puts(dir .."<br />") end

end
