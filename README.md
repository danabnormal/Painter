# Painter
Painter is a Windows program designed to replicate Ambilight functionality using ~~Philips Hue lightbulbs~~ other available technologies.

It is very likely that this will not be of use to you - this is being used to learn best practice etc for C# applictions.

## So what is the applications actual purpose?
The application is intended to try and replicate Ambilight functionality found in various TVs (https://www.youtube.com/watch?v=11bvDEs0Mdc) with ~~Philips Hue 'networked' lighting products~~ any similar technology.

The application should provide this functionality for anything that is shown within Windows, be that visualizations, media or games etc.

You'll notive some ~~strikethroughs~~ above. This is because if I do Best Practices right, I should be able to implement an extendable plugin architecture so not just Philips Hue is catered for...

## Whats this about Best Practice?
My experience is with VB.Net under ASP.Net and WinForms. This needs to change.

So I decided to give C# a blast with this concept. I actually have a running exe that performs tis programs purpose, but a poper coder will have a heart attack looking at the source, so I'm using this as a way to learn and force best practices down m throat.

This includes - but is not limited to - the following:

* WPF instead of WinForms
* Naming conventions
* Code comments
* cAsE cOnVeNtIoNs
* Use of Application and User settings
* Resource files to allow localis[z]ation
* 10-15 lines per function
* ALL THE THINGS should only be done once
* Objects
* Stop using globals(?)
* Stop using GOTO (JOKE joke joke)
* UI agnostic - GUI provided but should be easy to create a CLI for (eg)
* ...

This means that the code will be constantly evolving, even though the opbject of the app may be achieved. 
