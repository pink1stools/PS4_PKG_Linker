--Get path to this script e.g C:\UniSererZ\www\lua_examples\
path = debug.getinfo(1,"S").source:match[[^@?(.*[\/])[^\/]-$]] 

-- Add script path to update package path
package.path = path .."?.lua;".. package.path

-- Example of require
require 'test5a'
require 'test5b'

-- Example of dofile
dofile(path .. "test5c.lua")
dofile(path .. "test5d.lua")

function handle(r)
  r.content_type = "text/html"

  r:puts (header .. "<br />")
  banner(r)
  menu(r)
  r:puts (footer .. "<br />")

end
