/* JQuery Watermark Light Plugin
 * Version 1.01
 * http://www.davidjrush.com/jqueryplugin/watermark/
 *
 * Copyright 2013, David J Rush
 * Dual licensed under the MIT or GPL Version 2 licenses.
 * http://www.opensource.org/licenses/mit-license.php
 * http://www.opensource.org/licenses/GPL-2.0
 */
(function ($) {
    var watermarkArray = [];
    var methods = {
        init: function (options) {
            return this.each(function (i) {
                var $this = $(this);
                var $mark = $this.attr('title');
                watermarkArray.push($mark);
                if ($this.is(':password')) {
                    var stringNewTB = '<input type="text" class="watermark marked password" value="' + $mark + '" />';
                    $this.wrap('<span />').after(stringNewTB).hide().removeClass('watermark');
                    $this.blur(function () {
                        if ($this.val().length == 0)
                            $this.hide().next().show();
                    }).next().focus(function () {
                        $(this).hide().prev().show().focus();
                    });
                }
                else if ($this.is(':text') || $this.is('[type=email]') || $this.is('textarea')) {
                    $this.blur(function () {
                        if ($this.val().length == 0)
                            $this.val(watermarkArray[i]).addClass('marked');
                    }).focus(function () {
                        if ($this.val() == watermarkArray[i] && $this.hasClass('marked'))
                            $this.val('').removeClass('marked');
                    })
                    if ($this.val().length < 1)
                        $this.val($mark).addClass('marked');
                }
            });
        },
        clearWatermarks: function () {
            return this.each(function (i) {
                if ($(this).hasClass('marked') && $(this).val() == watermarkArray[i])
                    $(this).val('');
            });
        }
    };

    $.fn.watermark = function (method) {
        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on jQuery.watermark');
        }
    }
})(jQuery);