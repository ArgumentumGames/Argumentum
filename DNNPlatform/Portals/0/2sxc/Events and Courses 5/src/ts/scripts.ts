declare let $2sxc: any;
import * as $ from 'jquery';
const validate = require("jquery-validation");

require('../styles/_styles.scss');

$(document).ready(function() {
  const validator = ($("form") as any).validate();

  jQuery.extend(($ as any).validator.messages, {
      required: $('.app-events-form').data('string-required'),
      email: $('.app-events-form').data('string-email')
  });

  $(".form-submit").click(function(e) { 
    if(validator.form()) {
      const inputs = $(".event-form :input");
      const obj: any = {};
      $.each(inputs, function(i: any, e: any) {
        const propName = e.name;
        obj[propName] = $(e).val();
      });

      const sxc = $2sxc(this);
      sxc.webApi.post("Form/ProcessForm", {}, obj).then(function() {
        $('.event-form').hide();
        $('.form-info-success').show();
        $('.form-submit').prop('disabled', true);
      }, function() {
        $('.form-info-error').show();
        $('.form-submit').prop('disabled', false);
      });
    }

    // Add and remove has-error classes
    $('.event-form .form-group').removeClass('has-error');
    $('.form-info-error').hide(); 
    for (let i=0;i<validator.errorList.length;i++){
        $(validator.errorList[i].element).parents('.form-group').addClass('has-error');
    }
  });
});