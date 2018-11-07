--
 local A1 = os.date("time: %X")
 local A2 = os.getenv("US_ROOTF_WWW")
 local A3 = debug.getinfo(1,"S").source -- Information to this file
 local A4 = debug.getinfo(1).source:match("@(.*)$") --Full path to this file
 local A5 = debug.getinfo(1,"S").source:match[[^@?(.*[\/])[^\/]-$]] -- Path to this file


function handle(r)
 r.content_type = "text/html"

   --Global outside of handler function
   r:puts("<b>Global outside of handler function </b><br />")
   r:puts("A1) = " .. A1  .. "<br />")
   r:puts("A2) = " .. A2  .. "<br />")
   r:puts("A3) = " .. A3  .. "<br />")
   r:puts("A4) = " .. A4  .. "<br />")
   r:puts("A5) = " .. A5  .. "<br />")

   --Inside of handler function
   r:puts("<br /><b>Inside of handler function</b> <br />")
   r:puts("B1) = " .. os.date("time: %X")        .. "<br />")
   r:puts("B2) = " .. os.getenv("US_ROOTF_WWW")  .. "<br />")
   r:puts("B3) = " .. debug.getinfo(1,"S").source  .. "<br />")
   r:puts("B4) = " .. debug.getinfo(1).source:match("@(.*)$") .. " <br />")
   r:puts("B5) = " .. debug.getinfo(1,"S").source:match[[^@?(.*[\/])[^\/]-$]] .. "<br />")

   --Specific to the handler function
   r:puts("<br /><b>Specific to the handler function</b> <br />")
   r:puts("C1) r.uri      = " .. r.uri               .. "<br />")
   r:puts("C2) r.hostname = " .. r.hostname          .. "<br />")
   r:puts("C3) r.method   = " .. r.method            .. "<br />")
   r:puts("C4) r.filename = " .. r.filename          .. "<br />")
 
   path = string.gsub(r.filename, "^(.+/)[^/]+$", "%1") -- Get path
   r:puts ("C5) Extract path = " .. path .. "<br />")
   r:puts("File name r.filename =  " .. r.filename .."<br />")

   --Search path for packages
   r:puts("<br /><b>Search path for packages</b> -  uses package.path<br />")
   r:puts (package.path .. "<br />")

   --Search path for packages
   r:puts("<br /><b>Search path for packages</b> -  uses package.cpath<br />")
   r:puts (package.cpath)

end
