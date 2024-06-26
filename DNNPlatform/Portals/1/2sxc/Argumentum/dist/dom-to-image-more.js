(function (global) {
    'use strict';

    const util = newUtil();
    const inliner = newInliner();
    const fontFaces = newFontFaces();
    const images = newImages();

    const cacheForResponses = [];


    var cachedFonts;
    // Default impl options
    const defaultOptions = {
        // Default is to fail on error, no placeholder
        imagePlaceholder: undefined,
        // Default cache bust is false, it will use the cache
        cacheBust: false,
        // Default cache fonts is true, it will resolve fonts first
        cachedFonts: false,
        // Default scroll fix is false, it will not try to fix scrollbars
        scrollFix: false,
        // Default scroll is visible, it will not try to hide scroll bar
        scrollVisible: true,
        // Use (existing) authentication credentials for external URIs (CORS requests)
        useCredentials: false
    };

    const domtoimage = {
        toSvg: toSvg,
        toPng: toPng,
        toJpeg: toJpeg,
        toBlob: toBlob,
        toPixelData: toPixelData,
        getFontsBefore: getFontsBefore,
        toCanvas: toCanvas,
        impl: {
            fontFaces: fontFaces,
            images: images,
            util: util,
            inliner: inliner,
            options: {}
        }
    };

    if (typeof exports === "object" && typeof module === "object")
        module.exports = domtoimage;
    else
        global.domtoimage = domtoimage;

    /**
     * Allow to get fonts before creating image
     */
    function getFontsBefore() {
        cachedFonts = fontFaces.resolveAll();
    }
    /**
     * @param {Node} node - The DOM Node object to render
     * @param {Object} options - Rendering options
     * @param {Function} options.filter - Should return true if passed node should be included in the output (excluding node means excluding it's children as well). Not called on the root node.
     * @param {Function} options.onclone - Callback function which is called when the Document has been cloned for rendering, can be used to modify the contents that will be rendered without affecting the original source document.
     * @param {String} options.bgcolor - color for the background, any valid CSS color value.
     * @param {Number} options.width - width to be applied to node before rendering.
     * @param {Number} options.height - height to be applied to node before rendering.
     * @param {Object} options.style - an object whose properties to be copied to node's style before rendering.
     * @param {Number} options.quality - a Number between 0 and 1 indicating image quality (applicable to JPEG only), defaults to 1.0.
     * @param {Number} options.scale - scale factor for image quality rendering (to unblurry JPEG and PNG output), default = unset = no scaling applied = 1.0.
     * @param {String} options.imagePlaceholder - dataURL to use as a placeholder for failed images, default behaviour is to fail fast on images we can't fetch
     * @param {Boolean} options.cacheBust - set to true to cache bust by appending the time to the request url
     * @param {Boolean} options.cachedFonts - set to true to download fonts before
     * @param {Boolean} options.scrollFix - try to fix scrollbars
     * @param {Boolean} options.scrollVisible - try to hide scroll bar
     * @param {Boolean} options.useCredentials - use (existing) authentication credentials for external URIs (CORS requests)
     * @param {String} options.corsProxyUrl - set to cors proxy url with url need to bypass as parameter in the end (ex: https://your-cors-bypass/<url>)
     * @return {Promise} - A promise that is fulfilled with a SVG image data URL
     **/
    function toSvg(node, options) {
        options = options || {};
        copyOptions(options);
        return Promise.resolve(node)
            .then(function (node) {
                return cloneNode(node, options.filter, true);
            })
            .then(options.cachedFonts ? embedFontsAlreadyDownloaded : embedFonts)
            .then(inlineImages)
            .then(backgroundImages)
            .then(applyOptions)
            .then(function (clone) {
                return makeSvgDataUri(clone,
                    options.width || util.width(node),
                    options.height || util.height(node)
                );
            });

        function applyOptions(clone) {
            if (options.bgcolor) clone.style.backgroundColor = options.bgcolor;
            if (options.width) clone.style.width = options.width + 'px';
            if (options.height) clone.style.height = options.height + 'px';

            if (options.style)
                Object.keys(options.style).forEach(function (property) {
                    clone.style[property] = options.style[property];
                });

            var onCloneResult = null;

            if (typeof options.onclone === "function")
                onCloneResult = options.onclone(clone);

            return Promise.resolve(onCloneResult)
                .then(function () {
                    return clone;
                });
        }
    }

    /**
     * @param {Node} node - The DOM Node object to render
     * @param {Object} options - Rendering options, @see {@link toSvg}
     * @return {Promise} - A promise that is fulfilled with a Uint8Array containing RGBA pixel data.
     * */
    function toPixelData(node, options) {
        return draw(node, options || {})
            .then(function (canvas) {
                return canvas.getContext('2d').getImageData(
                    0,
                    0,
                    util.width(node),
                    util.height(node)
                ).data;
            });
    }

    /**
     * @param {Node} node - The DOM Node object to render
     * @param {Object} options - Rendering options, @see {@link toSvg}
     * @return {Promise} - A promise that is fulfilled with a PNG image data URL
     * */
    function toPng(node, options) {
        return draw(node, options || {})
            .then(function (canvas) {
                return canvas.toDataURL();
            });
    }

    /**
     * @param {Node} node - The DOM Node object to render
     * @param {Object} options - Rendering options, @see {@link toSvg}
     * @return {Promise} - A promise that is fulfilled with a JPEG image data URL
     * */
    function toJpeg(node, options) {
        options = options || {};
        return draw(node, options)
            .then(function (canvas) {
                return canvas.toDataURL('image/jpeg', options.quality || 1.0);
            });
    }

    /**
     * @param {Node} node - The DOM Node object to render
     * @param {Object} options - Rendering options, @see {@link toSvg}
     * @return {Promise} - A promise that is fulfilled with a PNG image blob
     * */
    function toBlob(node, options) {
        return draw(node, options || {})
            .then(util.canvasToBlob);
    }

    /**
     * @param {Node} node - The DOM Node object to render
     * @param {Object} options - Rendering options, @see {@link toSvg}
     * @return {Promise} - A promise that is fulfilled with a canvas object
     * */
    function toCanvas(node, options) {
        options = options || {};
        return draw(node, options)
            .then(function (canvas) {
                if (options.style)
                    Object.keys(options.style).forEach(function (property) {
                        canvas.style[property] = options.style[property];
                    });

                return canvas;
            });
    }

    function copyOptions(options) {
        // Copy options to impl options for use in impl
        if (typeof (options.imagePlaceholder) === 'undefined') {
            domtoimage.impl.options.imagePlaceholder = defaultOptions.imagePlaceholder;
        } else {
            domtoimage.impl.options.imagePlaceholder = options.imagePlaceholder;
        }

        if (typeof (options.cacheBust) === 'undefined') {
            domtoimage.impl.options.cacheBust = defaultOptions.cacheBust;
        } else {
            domtoimage.impl.options.cacheBust = options.cacheBust;
        }
        if (typeof (options.cachedFonts) === 'undefined') {
            domtoimage.impl.options.cachedFonts = defaultOptions.cachedFonts;
        } else {
            domtoimage.impl.options.cachedFonts = options.cachedFonts;
        }
        if (typeof (options.scrollFix) === 'undefined') {
            domtoimage.impl.options.scrollFix = defaultOptions.scrollFix;
        } else {
            domtoimage.impl.options.scrollFix = options.scrollFix;
        }

        if (typeof (options.scrollFix) === 'undefined') {
            domtoimage.impl.options.scrollVisible = defaultOptions.scrollVisible;
        } else {
            domtoimage.impl.options.scrollVisible = options.scrollVisible;
        }
        if (typeof (options.useCredentials) === 'undefined') {
            domtoimage.impl.options.useCredentials = defaultOptions.useCredentials;
        } else {
            domtoimage.impl.options.useCredentials = options.useCredentials;
        }

        if (typeof options.corsProxyUrl === 'string') {
            domtoimage.impl.options.corsProxyUrl = options.corsProxyUrl;
        } else {
            domtoimage.impl.options.corsProxyUrl = null;
        }
    }

    function draw(domNode, options) {
        return toSvg(domNode, options)
            .then(util.makeImage)
            .then(util.delay(100))
            .then(function (image) {
                const scale = typeof (options.scale) !== 'number' ? window.devicePixelRatio : options.scale;
                const canvas = newCanvas(domNode, scale);
                const ctx = canvas.getContext('2d');
                if (image) {
                    ctx.scale(scale, scale);
                    ctx.drawImage(image, 0, 0);
                }
                return canvas;
            });

        function newCanvas(domNode, scale) {
            const canvas = document.createElement('canvas');
            canvas.width = (options.width || util.width(domNode)) * scale;
            canvas.height = (options.height || util.height(domNode)) * scale;

            if (options.bgcolor) {
                const ctx = canvas.getContext('2d');
                ctx.fillStyle = options.bgcolor;
                ctx.fillRect(0, 0, canvas.width, canvas.height);
            }

            return canvas;
        }
    }

    function cloneNode(node, filter, root) {
        if (!root && filter && !filter(node)) return Promise.resolve();

        return Promise.resolve(node)
            .then(makeNodeCopy)
            .then(function (clone) {
                return cloneChildren(node, clone, filter);
            })
            .then(function (clone) {
                return processClone(node, clone);
            });

        function makeNodeCopy(node) {
            if (node instanceof HTMLCanvasElement) return util.makeImage(node.toDataURL());
            if (node instanceof HTMLIFrameElement) return (node.contentDocument || node.contentWindow.document).children[0].cloneNode(true);
            return node.cloneNode(false);
        }

        function cloneChildren(original, clone, filter) {
            const children = original.tagName === 'use' ? copyShadowChild(original) : original.childNodes;
            if (children.length === 0) return Promise.resolve(clone);

            return cloneChildrenInOrder(clone, util.asArray(children), filter)
                .then(function () {
                    return clone;
                });

            function cloneChildrenInOrder(parent, children, filter) {
                let done = Promise.resolve();
                children.forEach(function (child) {
                    done = done
                        .then(function () {
                            return cloneNode(child, filter);
                        })
                        .then(function (childClone) {
                            if (childClone) parent.appendChild(childClone);
                        });
                });
                return done;
            }
        }
        function copyShadowChild(original) {
            const child = document.getElementById(original.href.baseVal.replace("#", ""));
            return [child.cloneNode(true)];
        }

        function processClone(original, clone) {
            if (!(clone instanceof Element) && (clone.tagName || "").toLowerCase() != "html") return clone;

            if (original instanceof HTMLIFrameElement) original = original.contentDocument.children[0];

            return Promise.resolve()
                .then(cloneStyle)
                .then(clonePseudoElements)
                .then(copyUserInput)
                .then(fixSvg)
                .then(function () {
                    return clone;
                });

            function cloneStyle() {
                copyStyle(window.getComputedStyle(original), clone.style);
                // temporary fix of: https://github.com/tsayen/dom-to-image/issues/125
                if (util.isChrome() && clone.style.marker && clone.tagName === 'line') {
                    clone.style.marker = '';
                }

                function copyFont(source, target) {
                    target.font = source.font;
                    target.fontFamily = source.fontFamily;
                    target.fontFeatureSettings = source.fontFeatureSettings;
                    target.fontKerning = source.fontKerning;
                    target.fontSize = source.fontSize;
                    target.fontStretch = source.fontStretch;
                    target.fontStyle = source.fontStyle;
                    target.fontVariant = source.fontVariant;
                    target.fontVariantCaps = source.fontVariantCaps;
                    target.fontVariantEastAsian = source.fontVariantEastAsian;
                    target.fontVariantLigatures = source.fontVariantLigatures;
                    target.fontVariantNumeric = source.fontVariantNumeric;
                    target.fontVariationSettings = source.fontVariationSettings;
                    target.fontWeight = source.fontWeight;
                }

                function copyStyle(source, target) {
                    target.fontStretch == '';
                    if (source.cssText) {
                        target.cssText = source.cssText;
                        copyFont(source, target); // here we re-assign the font props.
                    } else {
                        copyProperties(source, target);
                        target.fontStretch = 'normal';
                    }

                    function copyProperties(source, target) {
                        util.asArray(source).forEach(function (name) {
                            target.setProperty(
                                name,
                                source.getPropertyValue(name),
                                source.getPropertyPriority(name)
                            );
                        });
                    }
                }

                if (domtoimage.impl.options.scrollFix && !domtoimage.impl.options.scrollVisible && (
                    clone.style.overflow === "auto" ||
                    clone.style.overflowX === "auto" ||
                    clone.style.overflowY === "auto"
                )) {
                    clone.style.overflow = "hidden"
                }

                if (domtoimage.impl.options.scrollFix &&
                    (original.scrollTop || original.scrollLeft)) {
                    // Setup container for absolute positioning of children
                    clone.style.position = 'relative';
                    clone.style.overflow = 'hidden';
                    clone.style.width = original.offsetWidth + 'px';
                    clone.style.height = original.offsetHeight + 'px';
                    var scrollTopRemaining = original.scrollTop > 0 ? original.scrollTop : null;
                    var scrollLeftRemaining = original.scrollLeft > 0 ? original.scrollLeft : null;
                    var originalIsPositionRelative = originalStyle['position'] === 'relative';
                    var computedStylesCache = {};
                    var boundingRectCache = {};

                    // Loop through children and set position based on original
                    // childs position and original containers scroll position
                    for (var i = 0; i < clone.children.length; i++) {
                        var cloneChild = clone.children[i];
                        // Make sure this element is stylable
                        if (typeof (cloneChild) === 'undefined' ||
                            cloneChild === null ||
                            typeof (cloneChild.style) === 'undefined') {

                            continue;
                        }
                        var originalChildStyles = computedStylesCache[i] || window.getComputedStyle(original.children[i]);
                        computedStylesCache[i] = originalChildStyles;

                        // Set child to absolute positioning relative to parent (container)
                        cloneChild.style.position = 'absolute';

                        // Take into account the fact that there may be children which were already
                        // positioned absolute relative to its parent, thus we need to use the original position
                        if (originalIsPositionRelative && originalChildStyles['position'] === 'absolute') {
                            var top = parseInt(originalChildStyles['top']);
                            var left = parseInt(originalChildStyles['left']);
                            top = isNaN(top) ? 0 : top;
                            left = isNaN(left) ? 0 : left;
                            top -= original.scrollTop;
                            left -= original.scrollLeft;
                            cloneChild.style.top = top + 'px';
                            cloneChild.style.left = left + 'px';
                            continue;
                        }

                        var currentChildBoundingRect = boundingRectCache[i] || original.children[i].getBoundingClientRect();
                        boundingRectCache[i] = currentChildBoundingRect;

                        // Find last child that was not position absolute
                        var lastChild, lastChildIndex, lastChildBoundingRect;
                        for (lastChildIndex = i - 1; lastChildIndex >= 0; lastChildIndex--) {
                            var childStyles = computedStylesCache[lastChildIndex] || window.getComputedStyle(original.children[lastChildIndex]);
                            computedStylesCache[lastChildIndex] = childStyles;
                            if (childStyles['position'] !== 'absolute') {
                                lastChild = original.children[lastChildIndex];
                                break;
                            }
                        }

                        // If we found a child then subtract its height/width from the scroll position
                        if (typeof (lastChild) !== 'undefined') {
                            lastChildBoundingRect = boundingRectCache[lastChildIndex] || lastChild.getBoundingClientRect();
                            boundingRectCache[lastChildIndex] = lastChildBoundingRect;

                            // isStackingTop is true when elements are being displayed block
                            if (lastChildBoundingRect.top !== currentChildBoundingRect.top) {
                                // Subtract last child real height to get the next items position
                                scrollTopRemaining -= (currentChildBoundingRect.top - lastChildBoundingRect.top);
                            }
                            // isStackingLeft is true when elements are being displayed inline
                            if (lastChildBoundingRect.left !== currentChildBoundingRect.left) {
                                // Subtract the last child real width to get the next items position
                                scrollLeftRemaining -= (currentChildBoundingRect.left - lastChildBoundingRect.left);
                            }
                        }

                        // Set the childs position based on our current scroll position
                        cloneChild.style.top = -scrollTopRemaining + 'px';
                        cloneChild.style.left = -scrollLeftRemaining + 'px';
                    }
                }
            }

            function clonePseudoElements() {
                [':before', ':after'].forEach(function (element) {
                    clonePseudoElement(element);
                });

                function clonePseudoElement(element) {
                    const style = window.getComputedStyle(original, element);
                    const content = style.getPropertyValue('content');

                    if (content === '' || content === 'none') return;

                    const className = util.uid();
                    const currentClass = clone.getAttribute('class');
                    if (currentClass) {
                        clone.setAttribute('class', currentClass + ' ' + className);
                    }

                    const styleElement = document.createElement('style');
                    styleElement.appendChild(formatPseudoElementStyle(className, element, style));
                    clone.appendChild(styleElement);

                    function formatPseudoElementStyle(className, element, style) {
                        const selector = '.' + className + ':' + element;
                        const cssText = style.cssText ? formatCssText(style) : formatCssProperties(style);
                        return document.createTextNode(selector + '{' + cssText + '}');

                        function formatCssText(style) {
                            const content = style.getPropertyValue('content');
                            return style.cssText + ' content: ' + content + ';';
                        }

                        function formatCssProperties(style) {

                            return util.asArray(style)
                                .map(formatProperty)
                                .join('; ') + ';';

                            function formatProperty(name) {
                                return name + ': ' +
                                    style.getPropertyValue(name) +
                                    (style.getPropertyPriority(name) ? ' !important' : '');
                            }
                        }
                    }
                }
            }

            function copyUserInput() {
                if (original instanceof HTMLTextAreaElement) clone.innerHTML = original.value;
                if (original instanceof HTMLInputElement) clone.setAttribute("value", original.value);
            }

            function fixSvg() {
                if (!(clone instanceof SVGElement)) return;
                clone.setAttribute('xmlns', 'http://www.w3.org/2000/svg');

                if (!(clone instanceof SVGRectElement)) return;
                ['width', 'height', 'mask'].forEach(function (attribute) {
                    const value = clone.getAttribute(attribute);
                    if (!value) return;

                    clone.style.setProperty(attribute, value);
                });
            }
        }
    }

    function embedFonts(node) {
        return fontFaces.resolveAll()
            .then(function (cssText) {
                var styleNode = document.createElement('style');
                node.appendChild(styleNode);
                styleNode.appendChild(document.createTextNode(cssText));
                return node;
            });
    }

    function embedFontsAlreadyDownloaded(node) {
        return cachedFonts
            .then(function (cssText) {
                var styleNode = document.createElement('style');
                node.appendChild(styleNode);
                styleNode.appendChild(document.createTextNode(cssText));
                return node;
            });
    }

    function inlineImages(node) {
        return images.inlineAll(node)
            .then(function () {
                return node;
            });
    }

    function backgroundImages(node) {
        return images.backgroundAll(node)
            .then(function () {
                return node;
            });
    }

    function makeSvgDataUri(node, width, height) {
        return Promise.resolve(node)
            .then(function (node) {
                node.setAttribute('xmlns', 'http://www.w3.org/1999/xhtml');
                return new XMLSerializer().serializeToString(node);
            })
            .then(util.escapeXhtml)
            .then(function (xhtml) {
                return '<foreignObject x="0" y="0" width="100%" height="100%">' + xhtml + '</foreignObject>';
            })
            .then(function (foreignObject) {
                return '<svg xmlns="http://www.w3.org/2000/svg" width="' + width + '" height="' + height + '">' +
                    foreignObject + '</svg>';
            })
            .then(function (svg) {
                return 'data:image/svg+xml;charset=utf-8,' + svg;
            });
    }

    function newUtil() {
        return {
            escape: escape,
            parseExtension: parseExtension,
            mimeType: mimeType,
            dataAsUrl: dataAsUrl,
            isDataUrl: isDataUrl,
            isSrcAsDataUrl: isSrcAsDataUrl,
            canvasToBlob: canvasToBlob,
            resolveUrl: resolveUrl,
            getAndEncode: getAndEncode,
            uid: uid(),
            delay: delay,
            asArray: asArray,
            escapeXhtml: escapeXhtml,
            makeImage: makeImage,
            width: width,
            height: height,
            isChrome: isChrome,
        };

        function mimes() {
            /*
             * Only WOFF and EOT mime types for fonts are 'real'
             * see http://www.iana.org/assignments/media-types/media-types.xhtml
             */
            const WOFF = 'application/font-woff';
            const JPEG = 'image/jpeg';

            return {
                woff: WOFF,
                woff2: WOFF,
                ttf: 'application/font-truetype',
                eot: 'application/vnd.ms-fontobject',
                png: 'image/png',
                jpg: JPEG,
                jpeg: JPEG,
                gif: 'image/gif',
                tiff: 'image/tiff',
                svg: 'image/svg+xml'
            };
        }

        function parseExtension(url) {
            const match = /\.([^\.\/]*?)(\?|$)/gu.exec(url);
            if (match) return match[1];
            else return '';
        }

        function mimeType(url) {
            const extension = parseExtension(url).toLowerCase();
            return mimes()[extension] || '';
        }

        function isDataUrl(url) {
            return url.search(/^(data:)/u) !== -1;
        }

        function isSrcAsDataUrl(text) {
            const DATA_URL_REGEX = /url\(['"]?(data:)([^'"]+?)['"]?\)/;

            return text.search(DATA_URL_REGEX) !== -1;
        }
        function toBlob(canvas) {
            return new Promise(function (resolve) {
                const binaryString = window.atob(canvas.toDataURL().split(',')[1]);
                const length = binaryString.length;
                const binaryArray = new Uint8Array(length);

                for (let i = 0; i < length; i++)
                    binaryArray[i] = binaryString.charCodeAt(i);

                resolve(new Blob([binaryArray], {
                    type: 'image/png'
                }));
            });
        }

        function canvasToBlob(canvas) {
            if (canvas.toBlob)
                return new Promise(function (resolve) {
                    canvas.toBlob(resolve);
                });

            return toBlob(canvas);
        }

        function resolveUrl(url, baseUrl) {
            const doc = document.implementation.createHTMLDocument();
            const base = doc.createElement('base');
            doc.head.appendChild(base);
            const a = doc.createElement('a');
            doc.body.appendChild(a);
            base.href = baseUrl;
            a.href = url;
            return a.href;
        }

        function uid() {
            let index = 0;

            return function () {
                return 'u' + fourRandomChars() + index++;

                function fourRandomChars() {
                    /* see http://stackoverflow.com/a/6248722/2519373 */
                    return ('0000' + ((Math.random() * Math.pow(36, 4)) << 0).toString(36)).slice(-4);
                }
            };
        }

        function makeImage(uri) {
            if (uri === 'data:,') return Promise.resolve();
            return new Promise(function (resolve, reject) {
                const image = new Image();
                if (domtoimage.impl.options.useCredentials) {
                    image.crossOrigin = 'use-credentials';
                }
                image.onload = function () {
                    resolve(image);
                };
                image.onerror = reject;
                image.src = uri;
            });
        }

        function getAndEncode(url) {
            const TIMEOUT = 30000;
            var _url = url.split('?')[0];
            var _obj = cacheForResponses.find(function (el) {
                return el.url === _url;
            });
            if (_obj) return _obj.promise;

            if (domtoimage.impl.options.cacheBust) {
                // Cache bypass so we don't have CORS issues with cached images
                // Source: https://developer.mozilla.org/en/docs/Web/API/XMLHttpRequest/Using_XMLHttpRequest#Bypassing_the_cache
                url += (/\?/u.test(url) ? '&' : '?') + new Date().getTime();
            }

            var promise = new Promise(function (resolve) {
                const request = new XMLHttpRequest();

                request.onreadystatechange = done;
                request.ontimeout = timeout;
                request.responseType = 'blob';
                request.timeout = TIMEOUT;
                if (domtoimage.impl.options.useCredentials) {
                    request.withCredentials = true;
                }
                let parsedUrl = url;
                if (domtoimage.impl.options.corsProxyUrl) {
                    parsedUrl = domtoimage.impl.options.corsProxyUrl + url;
                }
                request.open('GET', parsedUrl, true);
                request.send();

                let placeholder;
                if (domtoimage.impl.options.imagePlaceholder) {
                    const split = domtoimage.impl.options.imagePlaceholder.split(/,/);
                    if (split && split[1]) {
                        placeholder = split[1];
                    }
                }

                function done() {
                    if (request.readyState !== 4) return;

                    if (request.status !== 200) {
                        if (placeholder) {
                            resolve(placeholder);
                        } else {
                            fail('cannot fetch resource: ' + url + ', status: ' + request.status);
                        }

                        return;
                    }

                    const encoder = new FileReader();
                    encoder.onloadend = function () {
                        const content = encoder.result.split(/,/u)[1];
                        //var dataUrl = util.dataAsUrl(content, util.mimeType(url) || request.getResponseHeader('Content-Type'));
                        //resolve(dataUrl);
                        resolve(content);
                    };
                    encoder.readAsDataURL(request.response);
                }

                function timeout() {
                    if (placeholder) {
                        resolve(placeholder);
                    } else {
                        fail('timeout of ' + TIMEOUT + 'ms occurred while fetching resource: ' + url);
                    }
                }

                function fail(message) {
                    console.error(message);
                    resolve('');
                }
            });
            _obj = {
                url: _url,
                promise: promise
            };
            cacheForResponses.push(_obj);
            return _obj.promise;
        }

        function dataAsUrl(content, type) {
            return 'data:' + type + ';base64,' + content;
        }

        function escape(string) {
            return string.replace(/([.*+?^${}()|\[\]\/\\])/gu, '\\$1');
        }

        function delay(ms) {
            return function (arg) {
                return new Promise(function (resolve) {
                    setTimeout(function () {
                        resolve(arg);
                    }, ms);
                });
            };
        }

        function asArray(arrayLike) {
            const array = [];
            const length = arrayLike.length;
            for (let i = 0; i < length; i++) array.push(arrayLike[i]);
            return array;
        }

        function escapeXhtml(string) {
            return string.replace(/%/g, "%25").replace(/#/g, '%23').replace(/\n/g, '%0A');
        }

        function width(node) {
            const leftBorder = px(node, 'border-left-width');
            const rightBorder = px(node, 'border-right-width');
            return node.scrollWidth + leftBorder + rightBorder;
        }

        function height(node) {
            const topBorder = px(node, 'border-top-width');
            const bottomBorder = px(node, 'border-bottom-width');
            return node.scrollHeight + topBorder + bottomBorder;
        }

        function px(node, styleProperty) {
            const value = window.getComputedStyle(node).getPropertyValue(styleProperty);
            return parseFloat(value.replace('px', ''));
        }

        function isChrome() {
            return /chrome/i.test(navigator.userAgent);
        }
    }

    function newInliner() {
        const URL_REGEX = /url\(['"]?([^'"]+?)['"]?\)/gu;

        return {
            inlineAll: inlineAll,
            shouldProcess: shouldProcess,
            impl: {
                readUrls: readUrls,
                inline: inline
            }
        };

        function shouldProcess(string) {
            return string.search(URL_REGEX) !== -1;
        }

        function readUrls(string) {
            const result = [];
            let match;
            while ((match = URL_REGEX.exec(string)) !== null) {
                result.push(match[1]);
            }
            return result.filter(function (url) {
                return !util.isDataUrl(url);
            });
        }

        function inline(string, url, baseUrl, get) {
            return Promise.resolve(url)
                .then(function (url) {
                    return baseUrl ? util.resolveUrl(url, baseUrl) : url;
                })
                .then(get || util.getAndEncode)
                .then(function (data) {
                    return util.dataAsUrl(data, util.mimeType(url));
                })
                .then(function (dataUrl) {
                    return string.replace(urlAsRegex(url), '$1' + dataUrl + '$3');
                });

            function urlAsRegex(url) {
                return new RegExp('(url\\([\'"]?)(' + util.escape(url) + ')([\'"]?\\))', 'gu');
            }
        }

        function inlineAll(string, baseUrl, get) {
            if (nothingToInline() || util.isSrcAsDataUrl(string)) return Promise.resolve(string);

            return Promise.resolve(string)
                .then(readUrls)
                .then(function (urls) {
                    let done = Promise.resolve(string);
                    urls.forEach(function (url) {
                        done = done.then(function (string) {
                            return inline(string, url, baseUrl, get);
                        });
                    });
                    return done;
                });

            function nothingToInline() {
                return !shouldProcess(string);
            }
        }
    }

    function newFontFaces() {
        return {
            resolveAll: resolveAll,
            impl: {
                readAll: readAll
            }
        };

        function resolveAll() {
            return readAll(document)
                .then(function (webFonts) {
                    return Promise.all(
                        webFonts.map(function (webFont) {
                            return webFont.resolve();
                        })
                    );
                })
                .then(function (cssStrings) {
                    return cssStrings.join('\n');
                });
        }

        function readAll() {
            return Promise.resolve(util.asArray(document.styleSheets))
                .then(loadExternalStyleSheets)
                .then(getCssRules)
                .then(selectWebFontRules)
                .then(function (rules) {
                    return rules.map(newWebFont);
                });

            function selectWebFontRules(cssRules) {
                return cssRules
                    .filter(function (rule) {
                        return rule.type === CSSRule.FONT_FACE_RULE;
                    })
                    .filter(function (rule) {
                        return inliner.shouldProcess(rule.style.getPropertyValue('src'));
                    });
            }

            function loadExternalStyleSheets(styleSheets) {
                return Promise.all(
                    styleSheets.map(function (sheet) {
                        if (sheet.href) {
                            return fetch(sheet.href)
                                .then(toText)
                                .then(setBaseHref(sheet.href))
                                .then(toStyleSheet);
                        } else {
                            return Promise.resolve(sheet);
                        }
                    })
                );

                function toText(response) {
                    return response.text();
                }

                function setBaseHref(base) {
                    base = base.split('/');
                    base.pop();
                    base = base.join('/');

                    return function (text) {
                        return util.isSrcAsDataUrl(text) ? text : text.replace(
                            /url\(['"]?([^'"]+?)['"]?\)/g,
                            addBaseHrefToUrl
                        );
                    };

                    function addBaseHrefToUrl(match, p1) {
                        var url = /^http/i.test(p1) ?
                            p1 : concatAndResolveUrl(base, p1)
                        return 'url(\'' + url + '\')';
                    }

                    // Source: http://stackoverflow.com/a/2676231/3786856
                    function concatAndResolveUrl(url, concat) {
                        var url1 = url.split('/');
                        var url2 = concat.split('/');
                        var url3 = [];

                        processRelativeUrl(url1, url3);
                        processRelativeUrl(url2, url3);

                        function processRelativeUrl(urlToProcess, existUrl) {
                            for (var i = 0; i < urlToProcess.length; i++) {
                                // Always keep the domain (will be the first 3 items in the exist url)
                                if (urlToProcess[i] === '..' && existUrl.length > 3) {
                                    existUrl.pop();
                                } else if (urlToProcess[i] === '.') {
                                    continue;
                                } else {
                                    existUrl.push(urlToProcess[i]);
                                }
                            }
                        }
                        return url3.join('/');
                    }
                }

                function toStyleSheet(text) {
                    var doc = document.implementation.createHTMLDocument('');
                    var styleElement = document.createElement('style');

                    styleElement.textContent = text;
                    doc.body.appendChild(styleElement);

                    return styleElement.sheet;
                }
            }

            function getCssRules(styleSheets) {
                const cssRules = [];
                styleSheets.forEach(function (sheet) {
                    if (Object.getPrototypeOf(sheet).hasOwnProperty("cssRules")) {
                        try {
                            util.asArray(sheet.cssRules || []).forEach(cssRules.push.bind(cssRules));
                        } catch (e) {
                            console.log('Error while reading CSS rules from ' + sheet.href, e.toString());
                        }
                    }
                });
                return cssRules;
            }

            function newWebFont(webFontRule) {
                return {
                    resolve: function resolve() {
                        const baseUrl = (webFontRule.parentStyleSheet || {}).href;
                        return inliner.inlineAll(webFontRule.cssText, baseUrl);
                    },
                    src: function () {
                        return webFontRule.style.getPropertyValue('src');
                    }
                };
            }
        }
    }

    function newImages() {
        return {
            inlineAll: inlineAll,
            backgroundAll: backgroundAll,
            impl: {
                newImage: newImage
            }
        };

        function newImage(element) {
            return {
                inline: inline
            };

            function inline(get) {
                if (util.isDataUrl(element.src)) return Promise.resolve();

                return Promise.resolve(element.src)
                    .then(get || util.getAndEncode)
                    .then(function (data) {
                        return util.dataAsUrl(data, util.mimeType(element.src));
                    })
                    .then(function (dataUrl) {
                        return new Promise(function (resolve, reject) {
                            element.onload = resolve;
                            // for any image with invalid src(such as <img src />), just ignore it
                            element.onerror = reject;
                            if (element instanceof SVGImageElement) {
                                element.href.baseVal = dataUrl;
                            } else {
                                element.src = dataUrl;
                            }
                        });
                    });
            }

            function inlinehref(get) {
                const href = element.getAttributeNS('http://www.w3.org/1999/xlink', 'href');

                if (util.isDataUrl(href)) return Promise.resolve();

                if (!href) {
                    return Promise.resolve();
                }

                return Promise.resolve(href)
                    .then(get || util.getAndEncode)
                    .then(function (data) {
                        return util.dataAsUrl(data, util.mimeType(href));
                    })
                    .then(function (dataUrl) {
                        return new Promise(function (resolve, reject) {
                            element.onload = resolve;
                            // for any image with invalid src(such as <img src />), just ignore it
                            element.onerror = resolve;
                            element.setAttributeNS('http://www.w3.org/1999/xlink', 'href', dataUrl);
                        });
                    });
            }
        }

        function inlineAll(node) {
            if (!(node instanceof Element)) return Promise.resolve(node);

            return inlineBackground(node)
                .then(function () {
                    if (node instanceof HTMLImageElement || node instanceof SVGImageElement) {
                        return newImage(node).inline();
                    } else if (node.tagName === 'image') {
                        return newImage(node).inlinehref();
                    } else {
                        return Promise.all(
                            util.asArray(node.childNodes).map(function (child) {
                                return inlineAll(child);
                            })
                        );
                    }
                });

            function inlineBackground(node) {
                const background = node.style.getPropertyValue('background');

                if (!background) return Promise.resolve(node);

                return inliner.inlineAll(background)
                    .then(function (inlined) {
                        node.style.setProperty(
                            'background',
                            inlined,
                            node.style.getPropertyPriority('background')
                        );
                    })
                    .then(function () {
                        return node;
                    });
            }
        }

        function backgroundAll(node) {
            if (!(node instanceof Element)) return Promise.resolve(node);

            return inlineBackground(node)
                .then(function () {
                    if (node instanceof HTMLImageElement)
                        return newImage(node).inline();
                    else
                        return Promise.all(
                            util.asArray(node.childNodes).map(function (child) {
                                return backgroundAll(child);
                            })
                        );
                });

            function inlineBackground(node) {
                const background = node.style.getPropertyValue('background-image');

                if (!background) return Promise.resolve(node);

                return inliner.inlineAll(background)
                    .then(function (inlined) {
                        node.style.setProperty(
                            'background-image',
                            inlined,
                            node.style.getPropertyPriority('background-image')
                        );
                    })
                    .then(function () {
                        return node;
                    });
            }
        }
    }
})(this);