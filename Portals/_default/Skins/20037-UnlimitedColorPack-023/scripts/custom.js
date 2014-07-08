
//Top
	 jQuery(document).ready(function($) {
		$(window).scroll(function() {
		});	 
		jQuery('#to_top').click(function() {
			jQuery('body,html').animate({scrollTop:0},800);
		});	
	});

	jQuery(document).ready(function($) {		
	
		animatedcollapse.addDiv('mobile_menu', 'fade=1,speed=200,group=mobile,hide=1')
		animatedcollapse.addDiv('search', 'fade=1,speed=200,group=search,hide=1')
		animatedcollapse.ontoggle=function($, divobj, state){ //fires each time a DIV is expanded/contracted
			//$: Access to jQuery
			//divobj: DOM reference to DIV being expanded/ collapsed. Use "divobj.id" to get its ID
			//state: "block" or "none", depending on state
		}		
		animatedcollapse.init()
	});

//roll_menu
jQuery('#roll_nav').clingify({
    breakpoint: 0,  // in pixels
    extraClass: '',
    throttle: 100,  // in milliseconds
    distanceUp:300,
    // Callback functions:
    detached: $.noop,
    locked: $.noop,
    resized: $.noop
});
//Google Map
jQuery(document).ready(function($){
		jQuery('#gmap').gMap({
			address:'Bear city, ny ',
			maptype:'hybrid',
			zoom:8,
			scrollwheel:true,
			scaleControl:true,
			navigationControl:true,
			markers:[
				{address:'Bear city, ny ',html:'marker 1'},
				{address:' 579 Allen Road Basking Ridge, NJ 07920 ',html:'marker 1'},
				{address:' Mount Arlington, NJ 07856',html:'marker 1'}
				]
				
		});
});


//Windows Phone IE 10
jQuery(document).ready(function($){
		if (navigator.userAgent.match(/IEMobile\/10\.0/)) {
		 var msViewportStyle = document.createElement("style")
		  msViewportStyle.appendChild(
			document.createTextNode(
			  "@-ms-viewport{width:auto!important}"
			)
		  )
		  document.getElementsByTagName("head")[0].appendChild(msViewportStyle)
		}
});
