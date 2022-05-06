declare let Fancybox: any;
import { FancyboxOptions } from './lib-2sxc-fancybox-options';

/*
  This is a shared code used in various 2sxc apps. Make sure that they are in sync, so if you improve it, improve all 2sxc apps which use this. 
  ATM they are:
  - Gallery7
  - Content
  - ImageHotspots3
  The master with the newest / best version must always be the Gallery7, the others should then get a fresh copy.
  Because this is shared, all parameters like DOM-IDs etc. must be provided in the Init-call that it can work across apps
*/ 

export function initFancybox({ attribute, options } : { attribute: string, options: FancyboxOptions }) {
  Fancybox.bind(`[${attribute}]`, options);
}