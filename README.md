IronRubyMef
===========

Import and Export types to and from IronRuby for use with MEF -- the Managed Extensibility Framework for .NET

# Introduction

This is code from Nicholas Blumhardt's article [Ruby on MEF: Hybrid Application](http://blogs.msdn.com/b/nblumhardt/archive/2008/12/23/ruby-on-mef-hybrid-application.aspx).

The goal is for it to be like [IronPythonMef](http://github.com/jogoshugh/IronPythonMef)

I am attempting to upgrade it to .NET 4 and the latest version of MEF. 

So far I'm having issues with running it, [described here](http://www.ruby-forum.com/topic/4404638#1084495):

    Method not found: 'Microsoft.Scripting.Actions.Calls.OverloadInfo[] 
    Microsoft.Scripting.Actions.Calls.ReflectionOverloadInfo.CreateArray(System.Reflection.MemberInfo[])

If there are any IronRuby gurus out there who understand this messages, please help :)