function handle(r)
 -- Set Mime type to text/plain
  r.content_type = "text/plain"

 -- return the "hello World" text
  r:puts("Hello World")
end
