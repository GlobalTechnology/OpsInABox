// carousel
jQuery(function($) {
	
	function modifyDimensions(d){
		
		var imgWidth = $("#slide-photo.sel").width();
		var imgHeight = $("#slide-photo.sel").height();
		$('#window').width(imgWidth).height(imgHeight);
		
		if(d < 0){
			$("#next").css('left','409px');
		}
		else if(d > 0){
			$("#next").css('left','581px');
		}
		
		////I tried using the below code so that the slideshow would
		////support any size picture, however, the jQuery .css function
		////wouldn't read the variable. Hence, the hardcoded numbers seen 
		////above. Currently 450x300 and 622x342 size images can be used.
		/*	
		if(d < 0){
			var amount = 581 + d;
			var style = "'" + amount + "px'";
			$("#next").css('left',style);
		//	document.getElementById('next').style.left = style;
		}
		*/
		
	}
	
	var carousel = $('#carousel');
	var slider = $('#slides');
	var slides = slider.find('.slide');
	var controls = $('#controls');
	var numSlides = slides.length;
	var info = $('#info');
	var delay = 4000;
	
	var timer = null;
	
	var currSlide = -1;
	
	slides.each(function(i, slide) {
		var thumb = $(slide).data('thumb');
		
		$('<a href="#slide'+i+'" class="c"><img src="'+thumb+'" alt="" /></a>').css({left:(i * 75)}).click(function() {
			if (timer) {
				clearTimeout(timer);
				timer = null;
			}
			
			gotoSlide(i);
			return false;
		}).appendTo(controls);
	});
	var clone = slides.eq(0).clone();
	slider.append(clone);
	clone = slides.eq(numSlides - 1).clone();
	slider.prepend(clone);
	
	var next = $('<div id="next" class="nav"></div>').click(function() {
		if (timer) {
			clearTimeout(timer);
			timer = null;	
		}
		gotoNext(true);
		return false;
	}).hover(function() {
		$(this).stop(true).animate({marginLeft:5},{duration:70});
	}, function() {
		$(this).stop(true).animate({marginLeft:0},{duration:70});
	}).appendTo(carousel);
	var prev = $('<div id="prev" class="nav"></div>').click(function() {
		gotoPrev();
		
		return false;
	}).hover(function() {
		$(this).stop(true).animate({marginLeft:-5},{duration:70});
	}, function() {
		$(this).stop(true).animate({marginLeft:0},{duration:70});
	}).appendTo(carousel);
	
	gotoSlide(0, true);
	timer = setTimeout(function() {
		
		gotoNext();
	}, delay)
	
	function gotoNext(clicked) {
		if (timer) {
			clearTimeout(timer);
			timer = null;
		}
		
		if (currSlide < numSlides - 1) {
			gotoSlide(currSlide + 1);
			
		} else {
			var offset = slides.eq(currSlide).width();
			$.when(slider.stop(true).animate({left:('-='+offset)}, {duration:'fast'})).then(function() {
				gotoSlide(0, true);
			});
		}
		if (!clicked) {
			timer = setTimeout(function() {
				gotoNext();
			}, delay)
		}
	}
	function gotoPrev() {
		if (timer) {
			clearTimeout(timer);
			timer = null;
		}
		
		if (currSlide > 0) {
			gotoSlide(currSlide - 1);
		} else {
			var offset = slides.eq(currSlide).width();
			$.when(slider.stop(true).animate({left:('+='+offset)}, {duration:'fast'})).then(function() {
				gotoSlide(numSlides - 1, true)
			});
			
		}
	}
	
	function gotoSlide(i, jump) {
		
		if (i == currSlide) return;
		
		var prevSlide = currSlide;
		slides.eq(prevSlide).children('img').removeClass('sel');
		currSlide = i;
		
		var p = slides.eq(i).position();

		controls.find('> .c').eq(i).addClass('selected')
			.siblings().removeClass('selected');
		slides.eq(i).addClass('selected')
			.siblings().removeClass('selected');
		slides.eq(i).children('img').addClass('sel');
		
		var prevWidth = slides.eq(prevSlide).children('img').width();
		var newWidth = slides.eq(i).children('img').width();
		var diff = newWidth - prevWidth;
		
		modifyDimensions(diff);
			
			
		if (jump) {
			slider.stop(true).css({left:p.left * -1});
		} else {
			slider.stop(true).animate({left:p.left * -1}, {duration:'fast'});
		}
		
		info.hide().html(slides.eq(i).find('.info').html()).fadeIn()
		
		
		
	}
	
	

});