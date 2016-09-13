jQuery(function($) {/*'use strict',
	
	//Countdown js
	 $("#countdown").countdown({
         //02-Jun-16 12:00:00 AM
	     date: $('#event-banner.item.active').attr('xeventdate'),
			format: "on"
		},
		
		function() {
			// callback function
		});*/
	

	
	//Scroll Menu

	function menuToggle()
	{
		var windowWidth = $(window).width();

		if(windowWidth > 767 ){
			$(window).on('scroll', function(){
				if( $(window).scrollTop()>405 ){
					$('.main-nav').addClass('fixed-menu animated slideInDown');
				} else {
					$('.main-nav').removeClass('fixed-menu animated slideInDown');
				}
			});
		}else{
			
			$('.main-nav').addClass('fixed-menu animated slideInDown');
				
		}
	}

	menuToggle();
	
	
	// Carousel Auto Slide Off
	$('#event-carousel, #twitter-feed, #sponsor-carousel ').carousel({
		interval: false
	});


	// Contact form validation
	var form = $('.contact-form');
	form.submit(function () {'use strict',
		$this = $(this);
		$.post($(this).attr('action'), function(data) {
			$this.prev().text(data.message).fadeIn().delay(3000).fadeOut();
		},'json');
		return false;
	});

	$( window ).resize(function() {
		menuToggle();
	});

	//$('.main-nav ul').onePageNav({
	//	currentClass: 'active',
	//    changeHash: false,
	//    scrollSpeed: 900,
	//    scrollOffset: 0,
	//    scrollThreshold: 0.3,
	//    filter: ':not(.no-scroll)'
	//});

});


// Google Map Customization
(function(){

	
}());



