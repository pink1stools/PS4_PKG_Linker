function handle(r)
   r.content_type = "text/html"

r:puts [[
  <html>
  <head><title>Test 6</title></head>
  <body>
  <p>Lua processed data from form</p>
]]

   --Alternative 1
   allargs   = r:parseargs();
   firstname = allargs["firstname"]
   lastname  = allargs["lastname"]
   r:puts("Hi " .. firstname .. " " .. lastname .. "<br />")

   --Alternative 2
   for k, v in pairs(r:parseargs()) do
      if k=='firstname' then
         firstname=v
      elseif k=='lastname' then
         lastname=v
      end
   end
   r:puts(string.format("Hi %s %s", firstname, lastname))


r:puts [[
  </body></html>
]]
end

