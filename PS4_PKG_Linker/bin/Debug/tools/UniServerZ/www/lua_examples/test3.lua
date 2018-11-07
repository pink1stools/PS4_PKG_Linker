function handle(r)
 r.content_type = "text/html"

 -- return hello worlds text
 r:puts [[
  <h1>Hello Worlds  </h1>
  <h2>Hello World 1 </h2>
  Hello World 2     <br />
  Hello World 3     <br />
  Hello World 4
 ]]
end
