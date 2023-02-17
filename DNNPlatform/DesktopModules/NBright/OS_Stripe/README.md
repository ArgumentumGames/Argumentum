# OS_Stripe

## Installation

Install as a normal DNN module.

Go into the OpenStore Admin>Plugins.  You should see the Stripe plugin added.

Get the secretkey and publickey from your striope account and add them to the settings.


## Stripe WebHooks (IPN)

To complete the order you need an IPN from Stripe.  This event is "checkout.session.completed".

You need to setup a url to the notify.aspx that is the endpoint for the "checkout.session.completed" event in stripe.

Example of the url

https://my.openstore-ecommerce.com/DesktopModules/NBright/OS_Stripe/notify.ashx



How to do it: 

1. login to Stripe
2. Gotot Developers>WebHooks
3. Add (or Edit) Endpoint
4. Use your notify.aspx URL and link it to the "checkout.session.completed" event
5. Get you webhook secretkey. 
6. Add the webhook secret key to the Stripe settings in OpenStore.
