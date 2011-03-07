The razor-converter is a simple tool for converting Microsoft&reg; ASP.NET MVC WebForms Views to the new Razor syntax.
It was initially developed by the Telerik ASP.NET MVC team for internal use, but it now lives its own life on GitHub.

Known limitations:

* The tool only works with views and does not deal with the project structure and master pages.
* Expressions in script tags are not converted
* Due to the differences between the view engines the automatic conversion will sometimes fail or produce incorrect results. See the integration tests for specific scenarios that are not covered.

For general tips on converting WebForms Views to Razor see these blog posts:

* [Introducing Razor](http://weblogs.asp.net/scottgu/archive/2010/07/02/introducing-razor.aspx)
* [Model directive support](http://weblogs.asp.net/scottgu/archive/2010/10/19/asp-net-mvc-3-new-model-directive-support-in-razor.aspx)
* [Layouts](http://weblogs.asp.net/scottgu/archive/2010/10/22/asp-net-mvc-3-layouts.aspx)
* [Server-side comments](http://weblogs.asp.net/scottgu/archive/2010/11/12/asp-net-mvc-3-server-side-comments-with-razor.aspx)
* [The <text> syntax](http://weblogs.asp.net/scottgu/archive/2010/12/15/asp-net-mvc-3-razor-s-and-lt-text-gt-syntax.aspx)
* [Implicit and explicit code nuggets](http://weblogs.asp.net/scottgu/archive/2010/12/16/asp-net-mvc-3-implicit-and-explicit-code-nuggets-with-razor.aspx)
* [Introducing Razor (by Andrew Nurse)](http://blog.andrewnurse.net/2010/07/03/IntroducingRazorNdashANewViewEngineForASPNet.aspx)

[Help specific to the Telerik Extensions for ASP.NET MVC](http://www.telerik.com/help/aspnet-mvc/using-with-the-razor-view-engine.html)

Command line usage:

`aspx2razor <input file / wildcard> [output-directory] [options]`

Options:
  -r: Convert directories and their contents recursively (Contributed by Jeffrey T. Fritz)

A file with cshtml extension will be created for each input file.
Existing files will be OVERWRITTEN so be careful.
