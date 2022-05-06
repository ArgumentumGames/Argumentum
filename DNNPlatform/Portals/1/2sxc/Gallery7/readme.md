<image src="app-icon.png" align="right" width="200px">

# Gallery App V7 using Fancybox4 for .net CMSs

> This is a [2sxc](https://2sxc.org) App for [DNN ☢️](https://www.dnnsoftware.com/) and [Oqtane 💧](https://www.oqtane.org/)

A Gallery App using the [fancybox4](https://fancyapps.com/) javascript library.

| Aspect              | Status | Comments or Version |
| ------------------- | :----: | ------------------- |
| 2sxc                | ✅    | requires 2sxc v12.05
| Dnn                 | ✅    | For v7, v8 and v9
| Oqtane 2            | ✅    | Requires v2.02
| No jQuery           | ✅    | 
| Live Demo           | ➖    |
| Install Checklist   | ✅    | See [Installation](https://azing.org/2sxc/r/WLu6KUI4) on [azing.org](https://azing.org/2sxc)
| Source & License    | ✅    | included, ISC/MIT
| App Catalog         | ✅    | See [app catalog](https://2sxc.org/en/apps/app/gallery-app-v7-using-fancybox-4-hybrid-for-dnn-and-oqtane)
| Screenshots         | ✅    | See [app catalog](https://2sxc.org/en/apps/app/gallery-app-v7-using-fancybox-4-hybrid-for-dnn-and-oqtane)
| Best Practices      | ✅    | Uses v12.05 conventions
| Bootstrap 3         | ➖    | Not tested, but [v6.1](https://github.com/2sic/app-gallery/tree/v6-fancybox3) is
| Bootstrap 4         | ✅    |
| Bootstrap 5         | ✅    |


## Customize the App

Gallery App Fancybox7 uses the new, [standard Settings-System in 2sxc](http://r.2sxc.org/settings) to configure it. It also has app resources (i18n labels) you can customize.

If you want to customize the CSS, you will usually follow the ["Create Custom Styles in a Standard 2sxc App" checklist](https://azing.org/2sxc/r/Lu5SDBqU)


---

## History

### Version 6

Version 6 is found here: https://github.com/2sic/app-gallery/tree/v6-fancybox3

* Updated to conventions of 2sxc 12.0 as a Hybrid Dnn/Oqtane App based on [this checklist](https://azing.org/2sxc/r/m0iSLifK)

### Version 6.1 

1. Updated to features and conventions of 2sxc 12.03 based on [this checklist](https://azing.org/2sxc/r/KwXMhp8h)
1. Now uses the new `Settings.WebResources` and `Settings.Images` for global configuration
1. Renamed folder to enable side-by-side installation with older versions
1. Reset the GUID so it has a new ID now - allowing side-by-side installation with older versions
1. Dropped BS3 support - if anybody needs this, we recommend V5
1. Removed duplicate code, cleaned up everything
1. Albums can now also show Masonry-style galleries inside


### Version 7

1. Switched to Fancybox4
1. Dropped jQuery
1. Added turnOn to activate JS cleanly
1. Now uses dependencies from Settings.WebResources
1. Complete clean up / simplification of code files
1. Ensure that multiple galleries don't mix images in lightbox mode
1. Lots of documentation / comments everywhere