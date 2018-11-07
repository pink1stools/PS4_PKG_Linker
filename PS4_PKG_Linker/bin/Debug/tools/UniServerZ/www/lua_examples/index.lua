function handle(r)

 --Content type is the first line
 r.content_type = "text/html"

 --You can use the Lua heredoc [[ and ]] as follows:

r:puts [[

 <html>
  <head><title>Lua index example</title></head>
   <body>

  <h1>UniServerZ Lua examples</h1>

  <P>Basic Lua test pages.</P>
  <ul>
     <li><a href="http://localhost/lua_examples/test1.lua"  target="_blank">test1.lua - Basic "Hello world"</a> - handle() function</i>
     <li><a href="http://localhost/lua_examples/test2.lua"  target="_blank">test2.lua - Multi "Hello world"</a> - Multi-puts strings</i>
     <li><a href="http://localhost/lua_examples/test3.lua"  target="_blank">test3.lua - Multi "Hello world"</a> - Using Lua heredoc</i>
     <li><a href="http://localhost/lua_examples/test4.lua"  target="_blank">test4.lua - Code snippets </a> - Emphasis on file paths</i>
     <li><a href="http://localhost/lua_examples/test5.lua"  target="_blank">test5.lua - Correct paths for </a> - Directives require and dofile</i>
     <li><a href="http://localhost/lua_examples/test6.html" target="_blank">test6.html and test6.lua</a> - Basic form (GET) and process script</i>
     <li><a href="http://localhost/lua_examples/test7.html" target="_blank">test7.html and test7.lua</a> - Basic form (POST) and process script</i>
     <li><a href="http://localhost/lua_examples/test8.lua"  target="_blank">test8 - Read folder contents</a> - Uses the io.popen() function</i>
     <li><a href="http://localhost/lua_examples/test9.lua"  target="_blank">test9 - Read folder contents</a> - Uses the lfs.dll module</i>
     <li><a href="http://localhost/lua_examples/test10.lua"  target="_blank">test10 - Convert listed files to links</a> </i>
     <li><a href="http://localhost/lua_examples/guest_book/gb.lua"  target="_blank">test11 - Guest book</a> </i>
  </ul>

<p>The above test pages are located in folder <b>UniServerZ\www\lua_examples</b> after testing; this folder can be deleted:<br />
Main Test page index.lua and pages test1.lua to test11.lua</p>

  </body>
 </html>

]]


end
