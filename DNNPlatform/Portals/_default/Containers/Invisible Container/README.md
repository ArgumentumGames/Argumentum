# DNN Invisible Container
Invisible DNN Container - which is what is usually needed on most modern web sites. 

1. It works perfectly with DNN Bootstrap Skins like:
  1. [DNN Bootstrap3 Instant](https://github.com/2sic/dnn-theme-bootstrap3-instant)
  2. [DNN Bootstrap3 Single Pager](https://github.com/2sic/dnn-theme-bootstrap3-single-pager)
2. And it's awesome together with professional content-templates like
  1. [Standard Bootstrap3 Content Blocks/Templates](https://github.com/2sic/2sxc-content-bootstrap3)

## Background
If you want to create a good looking website, then the container of each module must be invisible. This is the ideal container for that. 

## Put it into Portals/\_default or /Portals/[your-portal]
We recommend that you place this in your /Portals/\_default area, but you can also put it into your specific portal, like /Portals/0/.
The following 2 examples assume you want to put it into \_default. 

## Quick-Install Using Git Command
This assumes that you already have git installed on your pc and that you kind of have an idea what git is :)

1. In your file explorer, go to `/Portals/_default/`, then shift-right-click on the `Containers` folder and choose _Open Command Window Here_. You should now have a command-line on that folder. 
2. Run this command: 
```
git clone https://github.com/2sic/dnn-container-invisible "Invisible container"
```

This will automatically get the latest copy of this container and place it in the right folder. You can now use it in any layout you want. 

## Installing using Windows File Explorer

1. In your file explorer, go to `/Portals/_default/Containers/`
2. Create a new folder, call it `Invisible container`. You should now have  
`/Portals/_default/Containers/Invisible container/`
3. Paste the ascx-file from [this package](https://github.com/2sic/dnn-container-invisible/archive/master.zip) into that folder. You should now have  
`/Portals/_default/Containers/Invisible container/default.ascx`

You can now use the container in any layout you want. 
