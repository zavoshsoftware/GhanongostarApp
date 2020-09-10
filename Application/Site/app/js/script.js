(function (window, document, $) {
	"use strict";
	
	$(window).on('load', function () {
		$("#loader-wrapper").fadeOut().remove();
	});
	
	/* On resize */
	$(window).on('resize', function () {
	});
	
	/* On scroll */
	$(window).on('scroll', function () {
		//Show or Hide back to top button
		if ($(window).scrollTop() >= 180) {
			$('#hook-to-top').addClass('hook_shown');
		} else {
			$('#hook-to-top').removeClass('hook_shown');
		}
		
		//menu on scroll
		if ($(this).scrollTop() > 150) {
			$('.header').addClass('scrolling-menu');
		} else {
			$('.header').removeClass('scrolling-menu');
		}
		
		//hook sticky menu on scroll
		if ($(this).scrollTop() > 850) {
			$('.hook-sticky-menu').addClass('appear');
		} else {
			$('.hook-sticky-menu').removeClass('appear');
		}
	});
	
	$(document).ready(function($) {
		var sidebar_is_open = false;
		//Init sidebar
		init_sidebar(sidebar_is_open);
		
		//Owl Carousel
		OwlCarousel();
		
		//Progress Bars
		progressBar();
		
		//Counters
		Counter();
		
		//Countdown Timer
		Countdown();
		
		//Ajax popup
		MagnificPopup();
		
		//Google Map
		googleMap();
		
		//Rotation Text
		initRotationText();
		
		//portfolio fullwidth background image
		portfolioFullwidthBg();
		
		//fitvids
		if ($('.media').length > 0) $('.media').fitVids();
		
		//fullscreen
		introHeight();
		
		//Equal Heights
		if ($('.equalheight').length > 0) $('.equalheight').equalHeights();
		
		//photography banner
		$('.photography-carousel .owl-pagination .owl-page').on('click touchstart', function() {
			var index = $(this).index();
			index = parseInt(index, 10);
			$(".photography-carousel-nav").find("li").removeAttr("class");
			$(".photography-carousel-nav").find("li").eq(index).addClass("active");
			
			var source = $(".photography-carousel-nav").find("li").eq(index).attr("data-source");
			$('#' + source).fadeIn();
			$('.section-hover').not('#' + source).fadeOut();
			if($('#' + source).find('video').length > 0) {
				$('#' + source).find('video').get(0).play();
			} else {
				$('.video-background-inner > video').get(0).pause();
			}
		});
		
		//banner carousel
		$('.banner-carousel .owl-pagination .owl-page').on('touchstart', function() {
			checkScreenSize($(this));
		});
		
		//scrolling
		var offset = $('.main-nav').attr('data-offset');
		var anchor_offset = $('#anchor').attr('data-offset');
		if ($('.main-nav').length > 0) $('.main-nav').singlePageNav({ 'offset': offset, 'filter': '.onepage' });
		if ($('#anchor, .anchor').length > 0) $('#anchor, .anchor').singlePageNav({ 'offset': anchor_offset, 'filter': '.onepage' });
		
		if (($("body, html").scrollTop() == 0) && ($(".main-nav .onepage").length > 0)) {
			$('.main-nav').find('li').children('a').removeClass('current');
			$('.main-nav').children('li').first().children('a').addClass('current');
		}
		
		if(!$(".no-headroom").length) {
			$(".header").headroom();
			$(".header-mobile").headroom();
			$(".hook-sticky-menu").headroom();
		}
		
		//scroll to top
		$('body').on('click', '#hook-to-top', function() {
			$("html, body").animate({
				scrollTop: 0
			}, 800);
			return false;
		});
		
		//banner carousel background image rotation
		$('.section-hover').hide();
		$('.section-hover').first().show();
		
		$(".banner-carousel .item").on("mouseenter", function() {
			var source = $(this).attr("data-source");
			$('#' + source).fadeIn();
			$('.section-hover').not('#' + source).fadeOut();
			if($('#' + source).find('video').length > 0) {
				$('#' + source).find('video').get(0).play();
			}
		});
		
		$(".banner-carousel .item").on("mouseout", function() {
			$('.video-background-inner > video').get(0).pause();
		});
		
		//Toggle Menu
		$('.overlay-menu').on('click', function() {
			$('body').toggleClass('show-search');
			$('.side-menu').removeClass('hidden');
			$('.main-menu').removeClass('hidden');
			if($('.side-menu').length > 0) {
				$(this).addClass('hidden');
			}
		});
		$('.side-menu').on('click', function() {
			$('body').toggleClass('show-search');
			$('.overlay-menu').toggleClass('hidden');
			$(this).addClass('hidden');
			$('.main-menu').addClass('hidden');
		});
		$('.menu-mobile .menu-item-has-children a').on('click', function() {
			$(this).parent('li').toggleClass('show');
		});
		
		$('.close-nav').on('touchstart', function(e) {
			if($('.hidden-bar').hasClass('open')) {
				$('.hidden-bar').removeClass('open');
				$('body').removeClass("prk_shifted");
				$('body').removeClass("showing_hidden_sidebar");
			}
			
		});
		
		//Toggle Accordion
		$(document).on('show.bs.collapse hide.bs.collapse', '.accordion', function(e) {
			var $target = $(e.target)
			if (e.type == 'show')
				$target.prev('.accordion-heading').find('.accordion-toggle').addClass('active');
			if (e.type == 'hide')
				$target.prev('.accordion-heading').find('.accordion-toggle').removeClass('active');
		});
		
		/* init revolution slider */
		if ($("#rev_slider").length > 0) {
			RevolutionInit();
		}
		
		$(document).ajaxComplete(function(event, request, settings) {
			//Ajax popup
			MagnificPopup();
			
			//fitvids
			if ($('.media').length > 0) $('.media').fitVids();
			
			//fullscreen
			introHeight();
		});
	});

})(window, document, jQuery);


/*=================================================================
	check screen size
===================================================================*/
function checkScreenSize(obj){
	var newWindowWidth = $(window).width();
	if (newWindowWidth < 769) {
		
		var index = obj.index();
		index = parseInt(index, 10);
		$(".banner-carousel-nav").find("li").removeAttr("class");
		$(".banner-carousel-nav").find("li").eq(index).addClass("active");
		
		var source = $(".banner-carousel-nav").find("li").eq(index).attr("data-source");
		$('#' + source).fadeIn();
		$('.section-hover').not('#' + source).fadeOut();
		if($('#' + source).find('video').length > 0) {
			$('#' + source).find('video').get(0).play();
		} else {
			$('.video-background-inner > video').get(0).pause();
		}
	}
}

/*=================================================================
	portfolio fullwidth
===================================================================*/
function portfolioFullwidthBg() {
	$('.portfolio-fullwidth').each(function() {
		$(this).css('background-image', 'url("' + $(this).attr("data-src") + '")');
	});
}

/*=================================================================
	owl carousel
===================================================================*/
function OwlCarousel() {
	if ($(".testimonials-slider").length > 0) {
		$(".testimonials-slider").each(function() {
			$(this).owlCarousel({
				navigation: false,
				slideSpeed: 600,
				pagination: true,
				paginationSpeed: 400,
				autoHeight: true,
				addClassActive: true,
				autoPlay: true,
				singleItem: true,
				transitionStyle : "backSlide",
			});
		});
	}
	if ($(".banner-carousel").length > 0) {
		$(".banner-carousel").each(function(){
			var autoplay = ($(this).attr("data-auto-play") === "true") ? true : false;
			$(this).owlCarousel({
				items: $(this).attr("data-desktop"),
				loop: true,
				mouseDrag: false,
				touchDrag: false,
				navigation: true,
				dots: false,
				pagination: true,
				autoPlay: autoplay,
				autoplayTimeout: 5000,
				autoplayHoverPause: true,
				smartSpeed: 1000,
				autoplayHoverPause: true,
				itemsDesktop: [1199, $(this).attr("data-desktop")],
				itemsDesktopSmall: [979, $(this).attr("data-laptop")],
				itemsTablet: [768, $(this).attr("data-tablet")],
				itemsMobile: [479, $(this).attr("data-mobile")]
			});
		});
	}
	if ($(".photography-carousel").length > 0) {
		$(".photography-carousel").each(function(){
			var autoplay = ($(this).attr("data-auto-play") === "true") ? true : false;
			$(this).owlCarousel({
				items: $(this).attr("data-desktop"),
				loop: true,
				mouseDrag: false,
				navigation: true,
				dots: false,
				pagination: true,
				autoPlay: autoplay,
				autoplayTimeout: 5000,
				autoplayHoverPause: true,
				smartSpeed: 1000,
				autoplayHoverPause: true,
				singleItem: true,
				transitionStyle : "backSlide",
			});
		});
	}
	if ($(".portfolio-slider").length > 0) {
		$(".portfolio-slider").each(function() {
			$(this).owlCarousel({
				navigation: false,
				slideSpeed: 600,
				pagination: true,
				paginationSpeed: 400,
				autoHeight: true,
				addClassActive: true,
				autoPlay: true,
				singleItem: true,
				afterInit: makePages,
				afterUpdate: makePages,
				transitionStyle : "fade",
			});
		});
	}
	if ($(".product-slider").length > 0) {
		$(".product-slider").each(function() {
			$(this).owlCarousel({
				navigation: false,
				slideSpeed: 600,
				pagination: true,
				paginationSpeed: 400,
				autoHeight: true,
				addClassActive: true,
				autoPlay: false,
				singleItem: true,
				afterInit: makePages,
				afterUpdate: makePages,
				transitionStyle : "backSlide",
			});
		});
	}
	if ($(".client-carousel").length > 0) {
		$(".client-carousel").each(function(){
			var autoplay = ($(this).attr("data-auto-play") === "true") ? true : false;
			$(this).owlCarousel({
				items: $(this).attr("data-desktop"),
				loop: true,
				mouseDrag: true,
				navigation: false,
				dots: false,
				pagination: false,
				autoPlay: autoplay,
				autoplayTimeout: 5000,
				autoplayHoverPause: true,
				smartSpeed: 1000,
				autoplayHoverPause: true,
				itemsDesktop: [1199, $(this).attr("data-desktop")],
				itemsDesktopSmall: [979, $(this).attr("data-laptop")],
				itemsTablet: [768, $(this).attr("data-tablet")],
				itemsMobile: [479, $(this).attr("data-mobile")]
			});
		});
	}
	
	//Twitter Slider
	if ($(".twitter-slider").length > 0) {
		$('.twitter-slider').each(function() {
			var $this_slider = $(this);
			$this_slider.flexslider({
				animation: "fade",
				useCSS: false,
				slideshow: true,
				slideshowSpeed: 5000,
				animationDuration: 300,
				smoothHeight: true,
				directionNav: true,
				controlNav: false,
				keyboardNav: false,
				touchDrag: false,
				prevText: '<i class="fa fa-chevron-left prk_less_opacity"></i>',
				nextText: '<i class="fa fa-chevron-right prk_less_opacity"></i>',
				start: function(slider) {
					slider.css({ 'min-height': 0 });
					$(window).trigger("debouncedresize");
				}
			});
		});
	}
	if ($(".twt-slider").length > 0) {
		$(".twt-slider").each(function() {
			$(this).owlCarousel({
				navigation: true,
				slideSpeed: 600,
				pagination: false,
				dots: false,
				paginationSpeed: 400,
				autoHeight: true,
				addClassActive: true,
				autoPlay: true,
				singleItem: true,
				transitionStyle : "fade",
				navigationText: ['<i class="mdi-chevron-left"></i>', '<i class="mdi-chevron-right"></i>'],
			});
		});
	}
}

function makePages() {
	$.each(this.owl.userItems, function(i) {
		$('.owl-controls .owl-page').eq(i)
			.css({
				'background': 'url(' + $(this).find('img').attr('src') + ') center center',
				'background-size': 'cover'
			})
	});
}

/*=================================================================
	rotation text
===================================================================*/
function initRotationText() {
	if ($('#js-rotating').length > 0) {
		$("#js-rotating").Morphext({
			animation: "flipInX",
			// An array of phrases to rotate are created based on this separator. Change it if you wish to separate the phrases differently (e.g. So Simple | Very Doge | Much Wow | Such Cool).
			separator: ",",
			// The delay between the changing of each phrase in milliseconds.
			speed: 2000,
			complete: function() {
				// Called after the entrance animation is executed.
			}
		});
	}
}

/*=================================================================
	progress bars
===================================================================*/
function progressBar() {
	$('.block-progressbar').each(function() {
		$(this).find('.progressbar').progressbar({ display_text: 'center' });
		$(this).find('.progressbar').css('background-color', $(this).attr('data-color'));
		$(this).find('.progress-title').css('color', $(this).attr('data-color'));
		$(this).find('.progressbar-front-text').css('color', $(this).attr('data-color'));
	});
}

/*=================================================================
	counters
===================================================================*/
function Counter() {
	if ($('.counter-wraper').length > 0) {
		$('.counter-wraper').each(function(index) {
			var $this = $(this);
			var waypoint = $this.waypoint({
				handler: function(direction) {
					$this.find('.counter-digit:not(.counted)').countTo().addClass('counted');
				},
				offset: "90%"
			});
		});
	}
}

/*=================================================================
	countdown
===================================================================*/
function Countdown() {
	if ($(".pl-clock").length > 0) {
		$(".pl-clock").each(function() {
			var time = $(this).attr("data-time");
			$(this).countdown(time, function(event) {
				var $this = $(this).html(event.strftime('' + '<div class="countdown-item"><div class="countdown-item-value">%D</div><div class="countdown-item-label">Days</div></div>' + '<div class="countdown-item"><div class="countdown-item-value">%H</div><div class="countdown-item-label">Hours</div></div>' + '<div class="countdown-item"><div class="countdown-item-value">%M</div><div class="countdown-item-label">Minutes</div></div>' + '<div class="countdown-item"><div class="countdown-item-value">%S</div><div class="countdown-item-label">Seconds</div></div>'));
			});
		});
	}
}

/*=================================================================
	magnific popup
===================================================================*/
function MagnificPopup() {
	//Gallery
	if ($(".gallery-item").length > 0) {
		$('.gallery-item').magnificPopup({
			gallery: {
				enabled: true
			}
		});
	}
	
	//Ajax popup
	if ($(".mfp-ajax").length > 0) {
		$('.mfp-ajax').magnificPopup({
			type: 'ajax',
			// Delay in milliseconds before popup is removed
			removalDelay: 300,

			// Class that is added to popup wrapper and background
			// make it unique to apply your CSS animations just to this exact popup
			mainClass: 'mfp-fade'
		});
	}
	
	//Close popup
	$('.portfolio-close').on("click", function(e) {
		var magnificPopup = $.magnificPopup.instance;
		magnificPopup.close();
	});
}

/*=================================================================
	fullscreen
===================================================================*/
function introHeight() {
	var wh = $(window).height();
	$('.section-fullscreen').css({ height: wh });
	$('.fullheight').css({ height: wh });
}

/*=================================================================
	google map
===================================================================*/
function googleMap() {
	if ($("#googleMap").length > 0) {
		$obj = $("#googleMap");
		var myCenter = new google.maps.LatLng($obj.data("lat"), $obj.data("lon"));
		var myMaker = new google.maps.LatLng($obj.data("lat"), $obj.data("lon"));
		function initialize() {
			var mapProp = {
				center: myCenter,
				zoom: 16,
				scrollwheel: false,
				mapTypeControlOptions: {
					mapTypeIds: [ google.maps.MapTypeId.ROADMAP, "map_style" ]
				}
			};
			var map = new google.maps.Map(document.getElementById("googleMap"), mapProp);
			var marker = new google.maps.Marker({
				position: myMaker,
				icon: $obj.data("icon")
			});
			marker.setMap(map);
		}
		google.maps.event.addDomListener(window, "load", initialize);
	}
}

/*=================================================================
	toggle mobile menu
===================================================================*/
function init_sidebar(sidebar_is_open) {
	$('.hidden-bar-toggle').on('click', function(e) {
		prk_toggle_sidebar(sidebar_is_open);
	});
	$('.sidebar_opener a').on('click', function(e) {
		e.preventDefault();
		prk_toggle_sidebar(sidebar_is_open);
	});
}
function hasParentClass(e, classname) {
	if (e === document) {
		return false;
	}
	if (classie.has(e, classname)) {
		return true;
	}
	return e.parentNode && hasParentClass(e.parentNode, classname);
}
function prk_toggle_sidebar(sidebar_is_open) {
	if (sidebar_is_open === false) {
		$('.hidden-bar-toggle').removeClass('hover_trigger');
		sidebar_is_open = true;
		$('body').addClass('prk_shifted');
		$('.hidden-bar').css({ 'visibility': 'visible' });
		$('.hidden-bar').addClass("open");
		setTimeout(function() {
			document.addEventListener("click", function(evt) {
				console.log(evt);
				if (evt === 'close_flag' || hasParentClass(evt.target, 'hider_flag')) {
					if (sidebar_is_open === true) {
						prk_toggle_sidebar(sidebar_is_open);
					}
				}
				if (evt.target.className.includes("onepage")) {
					if (sidebar_is_open === true) {
						prk_toggle_sidebar(sidebar_is_open);
					}
				}
			});
			$('#body_hider').addClass('prk_shifted_hider');
			$('body').addClass('showing_hidden_sidebar');
		}, 300);
	} else {
		sidebar_is_open = false;
		$('body').removeClass('prk_shifted');
		$('body').removeClass('showing_hidden_sidebar');
		$('#body_hider').removeClass('prk_shifted_hider');
		setTimeout(function() {
			document.addEventListener("click", function(evt) {
				if (evt === 'close_flag' || hasParentClass(evt.target, 'hider_flag')) {
					if (sidebar_is_open === true) {
						prk_toggle_sidebar(sidebar_is_open);
					}
				}
				if (evt.target.className.includes("onepage")) {
					if (sidebar_is_open === true) {
						prk_toggle_sidebar(sidebar_is_open);
					}
				}
			});
			$('.hidden-bar').css({ 'visibility': 'hidden' });
		}, 300);
	}
}

/*=================================================================
	revolution slider function
===================================================================*/
function RevolutionInit() {
	$("#rev_slider").show().revolution({
		sliderType: "standard",
		sliderLayout: "fullscreen",
		dottedOverlay: "none",
		delay: 6000,
		navigation: {
			keyboardNavigation: "off",
			keyboard_direction: "horizontal",
			mouseScrollNavigation: "off",
			onHoverStop: "on",
			touch: {
				touchenabled: "on",
				swipe_threshold: 75,
				swipe_min_touches: 1,
				swipe_direction: "horizontal",
				drag_block_vertical: false
			},
			arrows: {
				style: "hebe",
				enable: true,
				hide_onmobile: true,
				hide_under: 600,
				hide_onleave: true,
				hide_delay: 200,
				hide_delay_mobile: 1200,
				left: {
					h_align: "left",
					v_align: "center",
					h_offset: 0,
					v_offset: 0
				},
				right: {
					h_align: "right",
					v_align: "center",
					h_offset: 0,
					v_offset: 0
				}
			}
		},
		responsiveLevels: [1240, 1024, 778, 480],
		visibilityLevels: [1240, 1024, 778, 480],
		gridwidth: [1920, 1024, 778, 480],
		gridheight: [960, 768, 960, 720],
		lazyType: "none",
		shadow: 0,
		spinner: "spinner0",
		stopLoop: "off",
		stopAfterLoops: -1,
		stopAtSlide: -1,
		shuffle: "off",
		autoHeight: "off",
		fullScreenAutoWidth: "off",
		fullScreenAlignForce: "off",
		fullScreenOffsetContainer: "",
		fullScreenOffset: "",
		disableProgressBar: "on",
		hideThumbsOnMobile: "off",
		hideSliderAtLimit: 0,
		hideCaptionAtLimit: 0,
		hideAllCaptionAtLilmit: 0,
		debugMode: false,
		fallbacks: {
			simplifyAll: "off",
			nextSlideOnWindowFocus: "off",
			disableFocusListener: false,
		}
	});
}