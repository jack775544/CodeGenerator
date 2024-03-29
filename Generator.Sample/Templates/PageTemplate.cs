﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 16.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Generator.Sample.Templates
{
    using System.Collections.Generic;
    using Sample.Metamodel;
    using Generator.Core.Templates;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "C:\Users\Jack\repo\CodeGenerator\Generator.Sample\Templates\PageTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "16.0.0.0")]
    public partial class PageTemplate : AbstractTemplate<Page>
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public override string TransformText()
        {
            this.Write("This page is called ");
            
            #line 5 "C:\Users\Jack\repo\CodeGenerator\Generator.Sample\Templates\PageTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.Name));
            
            #line default
            #line hidden
            this.Write("\r\n");
            return this.GenerationEnvironment.ToString();
        }
        
        #line 6 "C:\Users\Jack\repo\CodeGenerator\Generator.Sample\Templates\PageTemplate.tt"

	private readonly IEnumerable<Page> _pages;

	public PageTemplate(IEnumerable<Page> pages)
	{
		_pages = pages;
	}

	public override string OutputPath => $"Pages/{Model.Name}Page.cs";

	public override IEnumerable<Page> MapObjects()
	{
		return _pages;
	}

        
        #line default
        #line hidden
    }
    
}
