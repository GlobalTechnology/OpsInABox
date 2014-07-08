(function($) {

  /**
   * Copyright 2012, Digital Fusion
   * Licensed under the MIT license.
   * http://teamdf.com/jquery-plugins/license/
   *
   * @author Sam Sehnert
   * @desc A small plugin that checks whether elements are within
   *     the user visible viewport of a web browser.
   *     only accounts for vertical position, not horizontal.
   */

   $.fn.visible = function(partial) {

   	var $t        = $(this),
   	$w            = $(window),
   	viewTop       = $w.scrollTop(),
   	viewBottom    = viewTop + $w.height(),
   	_top          = $t.offset().top,
   	_bottom       = _top + $t.height(),
   	compareTop    = partial === true ? _bottom : _top,
   	compareBottom = partial === true ? _top : _bottom;
   	if($t.hasClass('visible')){
   		return false;
   	}

   	return ((compareBottom <= viewBottom) && (compareTop >= viewTop));
   };

   var addAnimation = function(element){
   	$(element).each(function(i, el){
   		var el = $(el);
   		if (el.visible(true)) {
				if(el.attr("data-width")){el.css('width',el.attr("data-width"));}					
				el.removeClass('visible').addClass('animated');
				
				} 
   	});
   }
   var checkVisible = function(element){
   	$(element).each(function(i, el) {
   		var el = $(el);
         if (el.visible(false)) {
   			el.addClass("visible"); 
   		} 
   	});   	
   }


//  $(window).load(function(){
// 	checkVisible('.animation');
//   });

	$(window).load(function(){
					 addAnimation('.animation');
	 });
	 

   $(window).scroll(function(event) {
	 	 addAnimation('.animation');
    });
		
		

})(jQuery);