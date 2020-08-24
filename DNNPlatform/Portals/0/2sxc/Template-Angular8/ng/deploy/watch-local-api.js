var cpx = require('cpx')
var edition = 'local';
var package = require('../package.json');

// This will publish the /dist folder to the appropriate location
let target = package.config.publish_path;
let publishPath = `${target}/${edition}`;
if (target !== `..`) {

  // Publishing api folder
  cpx.watch("../local/api/**/*.*", `${publishPath}/api`)
    .on('copy', (e) => console.log(`watching local/api: pushed at ${new Date} - '${e.dstPath}'`));
  console.log(`local/api is being watched for changes`);
} else {
  console.log(`target path ${target} is within the current solution, so we won't auto-sync the local/api folder`)
}
