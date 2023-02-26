<image src="app-icon.png" align="right" width="200px">

# Glossary 3 App for .net CMSs

> This is a [2sxc](https://2sxc.org) App for [DNN ☢️](https://www.dnnsoftware.com/) and [Oqtane 💧](https://www.oqtane.org/)

A term glossary app to use with 2sxc

| Aspect              | Status | Comments or Version |
| ------------------- | :----: | ------------------- |
| 2sxc                | ✅    | requires 2sxc v13.10
| Dnn                 | ✅    | For v7, v8 and v9
| Oqtane 2            | ✅    | Requires v2.02
| No jQuery           | ✅    | 
| Live Demo           | ➖    |
| Install Checklist   | ✅    | See [Installation](https://azing.org/2sxc/r/JPX0Etz7) on [azing.org](https://azing.org/2sxc)
| Source & License    | ✅    | included, ISC/MIT
| App Catalog         | ✅    | See [app catalog](https://2sxc.org/en/apps/app/glossary3-hybrid-for-dnn-and-oqtane)
| Screenshots         | ✅    | See [app catalog](https://2sxc.org/en/apps/app/glossary3-hybrid-for-dnn-and-oqtane)
| Best Practices      | ✅    | Uses v13.10 conventions
| Bootstrap 3         | ✔️    | 
| Bootstrap 4         | ✅    |
| Bootstrap 5         | ✅    | 

## Installing the app

You can install the app straight forward with the ["Installing Glossary" checklist](https://azing.org/2sxc/r/JPX0Etz7)

## Customize the App

The Glossary App has no app settings but some app resources you can change.

If you want to customize the CSS, you will usually follow the ["Create Custom Styles in a Standard 2sxc App" checklist](https://azing.org/2sxc/r/gg_aB9FD)

## History

* v03.00 2021-10
    * Updated to best-practices of 2sxc 12.05
    * Hybrid, so it works with Oqtane
    * Tested to look ok on Bootstrap5
* v03.01.01 2022-02
    * Set default view
    * Updated License
* v03.01.02 02022-03
    * Enabled data-optimizations
* v03.02.00 2022-04
    * Replaced data-enableoptimizations with IPageService.AssetAttribute()
* v03.03.00 2022-06 
    * Changed all base classes to their 2sxc 14 equivalents
    * Replaced all GetService<> with the new ServiceKit14
    * Updated Webpack
* v03.03.01 2022-08
    * Updated the lighspeed config