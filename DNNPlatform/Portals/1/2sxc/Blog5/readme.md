<image src="app-icon.png" align="right" width="200px">

# Blog 5 App for .net CMSs

> This is a [2sxc](https://2sxc.org) App for [DNN ☢️](https://www.dnnsoftware.com/) and [Oqtane 💧](https://www.oqtane.org/)

A standard blog app to use with 2sxc

| Aspect              | Status | Comments or Version |
| ------------------- | :----: | ------------------- |
| 2sxc                | ✅    | requires 2sxc v12.05
| Dnn                 | ✅    | For v7, v8 and v9
| Oqtane 2            | ✅    | Requires v2.02
| No jQuery           | ✅    | 
| Live Demo           | ➖    | [2sxc blog](https://2sxc.org/en/blog) [Mettlers blog](http://www.mettler.li/blog)
| Install Checklist   | ✅    | See [Installation](https://azing.org/2sxc/r/vgApEx0X) on [azing.org](https://azing.org/2sxc)
| Source & License    | ✅    | included, ISC/MIT
| App Catalog         | ✅    | See [app catalog](https://2sxc.org/en/apps/app/blog-v5-hybrid-for-dnn-and-oqtane)
| Screenshots         | ✅    | See [app catalog](https://2sxc.org/en/apps/app/blog-v5-hybrid-for-dnn-and-oqtane)
| Best Practices      | ✅    | Uses v12.05 conventions
| Bootstrap 3         | ➖    | Use Blog4 if you need it
| Bootstrap 4         | ✅    |
| Bootstrap 5         | ✅    |

## Customize the App

If you want to use the "Home top posts" template, you need to ["Configure Main-List and Details Page"](https://azing.org/2sxc/r/c42g7EjU) in the app settings.
There are also a lot of app resources you can adjust to your needs.

If you want to customize the CSS, you will usually follow the ["Create Custom Styles in a Standard 2sxc App" checklist](https://azing.org/2sxc/r/gg_aB9FD)

---

## History

### V5 2021-10

* Now hybrid and also works in Oqtane
* New folder `Blog5` to allow side-by side install with previous versions
* Guid Reset to allow side-by-side install with previous version
* Updated to 2sxc 12.03 features like using global settings, images etc.
* Dropped Bootstrap3 support because of the many changes - if you need that, use the previous version 4
* Updated Search-mechanisms to be mostly configuration and otherwise separate from Razor as is now v12 standard
* Simplified Razor names to new best practices
* Moved all razor to `bs4` to make it easier for Bootstrap5 which will come soon

### V5.01 2022-03

* Updated License
* Changes default view