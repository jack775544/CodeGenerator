﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 16.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Generator.CleanSample.Templates
{
    using System.Collections.Generic;
    using Generator.Core.Templates;
    using Generator.CleanSample.Metamodel;
    using Generator.Clean;
    using Generator.Comments;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "C:\Users\Jack\repo\CodeGenerator\Generator.CleanSample\Templates\HomePage.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "16.0.0.0")]
    public partial class HomePage : AbstractTemplate<object>
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public override string TransformText()
        {
            
            #line 7 "C:\Users\Jack\repo\CodeGenerator\Generator.CleanSample\Templates\HomePage.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(this.AddCleanHeader(CommentType.Xml)));
            
            #line default
            #line hidden
            this.Write("\r\n<html>\r\n<head>\r\n    <title>Student Home</title>\r\n</head>\r\n\r\n<body>\r\n    This is the home pages for all students\r\n    <ol>\r\n");
            
            #line 16 "C:\Users\Jack\repo\CodeGenerator\Generator.CleanSample\Templates\HomePage.tt"

       foreach (var page in _pages)
       {

            
            #line default
            #line hidden
            this.Write("        <li><a href=\"");
            
            #line 20 "C:\Users\Jack\repo\CodeGenerator\Generator.CleanSample\Templates\HomePage.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(page.StudentName));
            
            #line default
            #line hidden
            this.Write(".html\">");
            
            #line 20 "C:\Users\Jack\repo\CodeGenerator\Generator.CleanSample\Templates\HomePage.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(page.StudentName));
            
            #line default
            #line hidden
            this.Write("</a></li>\r\n");
            
            #line 21 "C:\Users\Jack\repo\CodeGenerator\Generator.CleanSample\Templates\HomePage.tt"

       }

            
            #line default
            #line hidden
            this.Write("    </ol>\r\n\r\n    <script>\r\n        console.log('Hello World');\r\n    </script>\r\n</body>\r\n</html>\r\n");
            return this.GenerationEnvironment.ToString();
        }
        
        #line 31 "C:\Users\Jack\repo\CodeGenerator\Generator.CleanSample\Templates\HomePage.tt"

	private readonly IEnumerable<Page> _pages;

	public HomePage(IEnumerable<Page> pages)
	{
		_pages = pages;
	}

	public override string OutputPath => "index.html";

	public override IEnumerable<object> MapObjects()
	{
		return new object[] {null};
	}

        
        #line default
        #line hidden
    }
    
}