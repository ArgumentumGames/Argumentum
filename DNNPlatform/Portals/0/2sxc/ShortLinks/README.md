<img align="right" src="app-icon.png" width="200px">

# ShortLink App for DNN - The Quick-Links Manager
Basically this is a URL shortener, similar to bit.ly or goo.gl - except that it's on your site, and under your control. The core features are:
    
1. It manages short url-keywords like "qrt" and if this link is used, it will redirect to another url
1. The redirect-url can contain placeholders like {key} in case the target wants to know the key
1. **There are 4 types of Redirects**
    1. Standard Redirect: just redirect to a given url, mapped to the key
    1. Group Redirect: redirect all keys in this group to another url - typically forwarding the key as well
    1. Retired Redirect: when a key is marked as retired, the user will be redirected to the retired link
    1. Fallback Redirect: this is when a key isn't known

## Basics Terms and Concepts

To get started, you need to understand the basics of a redirect-system.

1.  The **key** or **keyword** is the bit after the url, which identifies the short-link.  
    Example: _asl_ on `http://2qr.ch/asl`
2.  The **target** or **link target** is where the browser should go when this short-link is used.  
    Example: `http://2qr.ch/asl` would send you to _2sxc.org/en/apps/app/short-link-system-like-goo.gl-or-bit.ly-for-dnn_
3.  An _optional_ **short domain** can further help shorten your URLs 
Example: we from [2sic](https://www.2sic.com/) like to use `2qr.ch` as our main-short url for tiny QR-codes
4.  When this app uses a **forward-redirect** (optional) then it is just an intermediary, and passes the key to another system - for example when  - these cases assume that the target-system will resolve the key itself  
Example: "these 57 keys belong to the product catalog"