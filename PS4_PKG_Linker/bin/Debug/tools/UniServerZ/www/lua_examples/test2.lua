function handle(r)
  -- Note Mime type changed to text/html
  r.content_type = "text/html"

 -- return hello worlds text
  r:puts("<h1>Hello Worlds  </h1>")
  r:puts("<h2>Hello World 1 </h2>")
  r:puts("Hello World 2    <br />")
  r:puts("Hello World 3   <br /> ")
  r:puts("Hello World 4")
end
