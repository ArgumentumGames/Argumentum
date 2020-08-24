import { PolyFills } from './components/array_find';

export class App {
    constructor(moduleid: number) {
        this.main(moduleid);
    }

    main(moduleid: number) {
        // First, check if Array.find() is supportet, if not, use a polyfill.
        new PolyFills();

        // Build the app-wrapper, so this code only uses elements from this module.
        const appWrapper = `.app-${moduleid}`;

        // Attach click to all accordions when loading the site.
        $(`${appWrapper} .co-accordion-title`).click(function (e) {
            e.preventDefault();
            var _this = $(this);

            // Set URL-Hash on click.
            var hash = $(this).attr('id');
            location.hash = hash;

            $(this).toggleClass('active').next('.co-accordion-content').slideToggle('normal', function () {
                $(window).resize();
            });

            $('html, body').animate({
                scrollTop: _this.offset().top - ($('.role-admin').length > 0 ? 200 : 200)
            }, 500);
        });

        // Get hash from url and open the according questoin item.
        if (window.location.hash) {
            var hash = window.location.hash;
            if ($(hash).length > 0) {
                $(hash).click();

                $('html, body').animate({
                    scrollTop: $(hash).offset().top - ($('.role-admin').length > 0 ? 200 : 200)
                }, 800);
            }
        }

        // Define the crucial wrappers, so we can use them later on in the code.
        var wrapperParent = $(`${appWrapper} .question-wrapper-outer`);
        var wrapper = $(`${appWrapper} .question-wrapper`);
        var questionItems = wrapper.find('.co-accordion-item');
        var filter = '';

        $(`${appWrapper} .faq-category-element`).click(function () {
            // This checks if the current element has an active state.
            $(`${appWrapper} .faq-category-element`).each(function () {
                $(this).removeClass('active');
            });

            $(this).addClass('active');

            // This code is responsible for the filtering of the questions
            var newFilter = $(this).data('filter');

            // Stop if clicked on the same filter as the one that is currently active.
            if (newFilter === filter) return;

            filter = newFilter;

            wrapperParent.css('min-height', wrapperParent.height() + 'px');
            wrapper.css('opacity', 0);

            setTimeout(function () {
                questionItems.each(function () {
                    var filterElems = $(this).data('filterelem');
                    $(this).css('display', (filter === 'nofilter' || filterElems
                        .find(function (elem: string) {
                            return elem === filter;
                        })) ? 'block' : 'none');
                });

                wrapperParent.css('min-height', wrapper.height() + 'px');

                setTimeout(function () {
                    wrapper.css('opacity', 1);
                }, 200)
            }, 350);
        });
    }
}
