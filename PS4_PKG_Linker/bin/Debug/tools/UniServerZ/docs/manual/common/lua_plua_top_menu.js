// File from main documentation will have been read. Variable main_doc
// is defined hence no need to display this small menu the main menu will be displayed.
// var main_doc;
// document.write('<p>mm' + main_doc + 'mmm</p>');
//var main_doc;

if (typeof main_doc === 'undefined'){

document.write('<div id=\"lcontop\">');
document.write('<ul class=\"lmen1\">');

document.write('<li class=\"p1200\"><a href=\"lua.html\"   target=\"_top\">Lua - Apache mod_lua</a></li>');
document.write('<li class=\"p1300\"><a href=\"plua.html\"  target=\"_top\">Lua - Apache mod_plua</a></li>');

document.write('</ul>');
document.write('</div>');
}