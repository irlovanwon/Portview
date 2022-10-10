/* (C) fruitbatinshades 2008 onwards */

jQuery.fn.fbisCarousel = function(options) {
  // set up initial options
  settings = jQuery.extend({
     noToDisplay: 4, //no of items to display across
     adjustWidth: true, //whether to adjust the parent width on init
	 arrowSelector: null //use links outside our rendered html (extra buttons)
  }, options);
// iterate and initialise each matched element
  return this.each(function() {
    var $this = jQuery(this);
	var $items = $this.find("ul li")
	
    //bind scroll function to click
	$this.find("div.arrows a").click(function(e){scroll(e,$this);});
	
	if (settings.adjustWidth)
	{
		//images aren't available to get their width till the doc is loaded
		//so we ned to waitl till then to do the resize.
		$(window).load(function(){
			//set all items to be the same width as the first
			$items.width($items.eq(0).outerWidth());
			//set parent width to match items
			$this.width(($items.eq(0).outerWidth() * settings.noToDisplay)+2);
			//set arrow holder width to match parent div
			$this.find(".arrows").width($this.outerWidth());
		});
	}
	//extra arrows so bind then too
	if (settings.arrowSelector != null)
	{
		jQuery(settings.arrowSelector).click(function(e){scroll(e,$this);});
	}
  });
  function scroll(e, car)
	{
		var ul = jQuery(car.find("ul").get(0));
		var noOff = ul.find("li").size();
		//var index = parseInt(jQuery.data(ul,"index"));
		var index = parseInt(jQuery(ul).data("index"));
		var left = parseInt(ul.css('left'));
		
		//get the relevant widths
		itemwidth = car.find("ul li").eq(0).outerWidth();
		noDisplayed = Math.round(car.outerWidth() / itemwidth);
		
		if (isNaN(index))
		{
			//not yet set
			jQuery(ul).data("index",1);
			index=0;
		}
		
		if ($(e.target).hasClass('slideleft'))
		{
			if(index > 0) //make sure we are not at the start
			{
				index--;
				jQuery(ul).data("index",index);
				//move the ul
				ul.animate({left:(0 - (index * itemwidth)) + 'px'});
			}
		}
		if ($(e.target).hasClass('slideright'))
		{
			if(index + noDisplayed < (noOff )) // make sure we are not at the last item
			{
				index++;
				jQuery(ul).data("index",index);
				//move the ul
				ul.animate({left:(0 - (index * itemwidth)) + 'px'});
			}
		}
		//stop event
		e.preventDefault();
		return false;
	}
};
