$(document).ready(function ($) {
    "use strict";

    $(function () {
        $('#dnnbootsterNav').on('hidden.bs.collapse', function () {
            $('.navbar-toggler').removeClass('open');
        })
        $('#dnnbootsterNav').on('show.bs.collapse', function () {
            $('.navbar-toggler').addClass('open');
        })
    });

    $(function () {
        if ($('form').hasClass('showControlBar')) {
            $('.controlbarfix').addClass('admin');
            $('.header-spacer').addClass('header-spacer-admin');
        } else {
            $('.header-spacer').addClass('header-spacer');
        }
    });






    $('.dropdown-menu a.dropdown-toggle').on('click', function (e) {
        if (!$(this).next().hasClass('show')) {
            $(this).parents('.dropdown-menu').first().find('.show').removeClass("show");
        }
        var $subMenu = $(this).next(".dropdown-menu");
        $subMenu.toggleClass('show');


        $(this).parents('li.nav-item.dropdown.show').on('hidden.bs.dropdown', function (e) {
            $('.dropdown-submenu .show').removeClass("show");
        });


        return false;
    });






});

