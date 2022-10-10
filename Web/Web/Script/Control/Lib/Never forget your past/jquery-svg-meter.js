/**
 * jQuery SVG Meter Plugin 1.0
 *
 * This jQuery plugin requires Raphael 2.x
 *
 * License: WTFPL
 * Copyright (C) 2013 Terry Young <terryyounghk@gmail.com>
 */
(function ($, undefined) {

	/**
	 * Available methods of .svgMeter()
	 * @type {Object}
	 */
	var methods = {
		/**
		 * Initializes the svgMeter
		 * @param options
		 * @return {*} chaining
		 */
		init: function (options) {
			return this.each(function (i, el) {
				var instance = new SVGMeter($(el), options);
				$(el).data('svgmeter', instance);
			});
		},

		/**
		 * Returns the svgMeter instance
		 * @return {*}
		 */
		widget: function () {
			var instance = this.data('svgmeter');
			return instance;
		},

		/**
		 * Returns the Raphael paper object, in case you ever need to access it directly
		 */
		paper: function () {
			var instance = this.data('svgmeter');
			return instance.paper;
		},

		/**
		 * Sets/Gets the value of the svgMeter
		 *
		 * Obsolete. As we are going to duck-punch $.fn.val below, you can then just use $('#meter').val() to set/get the value.
		 *
		 * @param value
		 * @return {*}
		 */
		value: function (value) {
			var instance = this.data('svgmeter');
			if (typeof value === undefined) {
				return instance.val();
			} else {
				instance.val(value);
			}
		},

		/**
		 * Returns the minValue of the svgMeter
		 * @return {int}
		 */
		minValue: function () {
			var instance = this.data('svgmeter');
			return instance.minValue;
		},

		/**
		 * Returns the maxValue of the svgMeter
		 * @return {int}
		 */
		maxValue: function () {
			var instance = this.data('svgmeter');
			return instance.maxValue;
		},

		/**
		 * For demo. Sets a random value to the svgMeter
		 * @return {*}
		 */
		randomize: function () {
			return this.each(function (i, el) {
				$(el).data('svgmeter').randomize();
			});
		}
	};

	$.fn.svgMeter = function (method) {
		// Method calling logic
		if (methods[method]) {
			return methods[ method ].apply(this, Array.prototype.slice.call(arguments, 1));
		} else if (typeof method === 'object' || !method) {
			return methods.init.apply(this, arguments);
		} else {
			$.error('Method ' + method + ' does not exist on jQuery.svgMeter');
		}
	};

	// -----------------------------------------------------------------------------------------------------------------
	// DUCK PUNCH !!!!!!!!!!!!!!!!!!!!
	//
	// Usages:
	//
	// $('#meter').val(100); // Sets the value of the svgMeter to 100
	// alert($('#meter').val()); // alerts '100'
	//
	var _oldval = $.fn.val;
	$.fn.val = function (value) {
		if (value === undefined) {
			// return the first svgMeter's value
			var instance = this.first().data('svgmeter');
			if (instance instanceof SVGMeter) {
				return instance.val();
			} else {
				// else, let the original .val() kick in
				return _oldval.apply(this, arguments);
			}
		} else {
			// apply the value to all elements in the set
			return this.each(function (i, el) {
				var $el = $(el),
					instance = $el.data('svgmeter');

				if (instance instanceof SVGMeter) {
					instance.val(value);
				} else {
					_oldval.apply($el, [value]);
				}
			});
		}
	};
	//
	// QUACK !!!!!!!!!!!!!!!!!!!!
	// -----------------------------------------------------------------------------------------------------------------


	// -----------------------------------------------------------------------------------------------------------------
	// Beyond this line is the internal SVGMeter constructor and prototype
	// The default properties are within the SVGMeter's init() method
	//

	/**
	 * SVGMeter displays a meter-like widget, either horizontally or vertically
	 *
	 * @param jqObject
	 * @param options
	 * @constructor
	 */
	function SVGMeter(jqObject, options) {
		this.init();
		$.extend(true, this, this.defaults); // First extend the new instance with the defaults
		$.extend(true, this, options || {}); // Then extend the new instance with options, if specified, which overrides the defaults
		this.originalOptions = options; // reserved
		this.container = jqObject.get(0); // required by Raphael: http://raphaeljs.com/reference.html#Raphael
		this.create();
		this.draw();

		if (options && options['value'] !== undefined) {
			this.val(options.value);
		}
	}

	/**
	 * SVGMeter Prototype
	 * @type {Object}
	 */
	SVGMeter.prototype =
	{
		init: function () {

			if (!Raphael) {
				var msg = 'Raphael JavaScript Library is required for the jQuery svgMeter plugin.';
				return (window.console && console.warn(msg)) || alert(msg);
			} else if (!Raphael.type) {
				var msg = 'Your browser does not support vector graphics.';
				return (window.console && console.warn(msg)) || alert(msg);
			} else {
				this.defaults =
				{
					orientation: 'horizontal', // possible values are: 'horizontal', 'vertical'
					width:       300,
					height:      50,

					value:		0,
					minValue:   0,
					maxValue:   100,
					majorTicks: 10, // Number of major slots on the meter.
					minorTicks: 5, // Number of slots per majorTick. If you do not want minorTicks, set this to zero.

					hMargin: 10,
					vMargin: 5,

					vAlignment: 'bottom', // possible values are: 'top', 'bottom'
					hAlignment: 'right', // possible values are: 'left', 'right'

					fontColor:             '#000000',
					fontSize:              10,
					stepColor:             '#000000',
					majorTicksStrokeWidth: 2,
					majorTicksLength:      20,
					minorTicksStrokeWidth: 1,
					minorTicksLength:      10,
					meterStrokeWidth:      4,
					meterOpacity:          0.7,
					meterColor:            '#ff0000',

					speed:  800,
					easing: '<>'
				};

				this.value = this.defaults.value;
			}

			return true;
		},

		/**
		 * This function creates the Raphael paper
		 */
		create: function () {
			this.paper = Raphael(this.container, this.width, this.height);
		},

		/**
		 * This is a triage function which draws a horizontal/vertical meter
		 */
		draw: function () {
			if (this.orientation === 'horizontal') {
				this._drawHorizontalMeter();
			}
			if (this.orientation === 'vertical') {
				this._drawVerticalMeter();
			}
		},

		/**
		 * This function is called during instantiation, and draws a horizontal meter
		 * @private
		 */
		_drawHorizontalMeter: function () {
			var objMajorTicksStyle = {
					'stroke-width': this.majorTicksStrokeWidth,
					stroke:         this.stepColor
				},
				objMinorTicksStyle = {
					'stroke-width': this.minorTicksStrokeWidth,
					stroke:         this.stepColor
				},
				objTextStyle = {
					'text-anchor': 'middle',
					'font-size':   this.fontSize,
					fill:          this.fontColor
				},
				objMeterStyle = {
					fill:           this.meterColor,
					width:          this.meterStrokeWidth,
					opacity:        this.meterOpacity,
					'stroke-width': 0
				},
				iBasePos = (this.vAlignment === 'bottom')
					? this.height - this.vMargin
					: 0 + this.vMargin,
				iLeft = this.hMargin,
				iRight = this.width - this.hMargin,
				iWidth = this.width - this.hMargin * 2,
				objMajorTicks = this.paper.set(), // create Raphael set for major ticks
				objMinorTicks = this.paper.set(), // create Raphael set for minor ticks
				objLabels = this.paper.set(), // create Raphael set for major tick labels
				iTop = (this.vAlignment === 'bottom')
					? this.height - this.majorTicksLength - this.vMargin
					: 0 + this.vMargin,
				iMinorTop = (this.vAlignment === 'bottom')
					? this.height - this.minorTicksLength - this.vMargin
					: 0 + this.vMargin,
				iMinorBottom = (this.vAlignment === 'bottom')
					? this.height - this.vMargin
					: this.minorTicksLength + this.vMargin,
				iBottom = (this.vAlignment === 'bottom')
					? this.height - this.vMargin
					: this.majorTicksLength + this.vMargin,
				iTextPos = (this.vAlignment === 'bottom')
					? iTop - this.fontSize
					: this.majorTicksLength + this.fontSize,
				objMajorTickPath,
				objMinorTickPath,
				objText;

			this.basePath = this.paper
				.path('M' + iLeft + ',' + iBasePos + 'L' + iRight + ',' + iBasePos)
				.attr(objMajorTicksStyle);

			for (var i = 0,
					 j = this.majorTicks + 1,
					 k = iWidth / this.majorTicks,
					 m = iLeft,
					 n = this.minValue,
					 o = (this.maxValue - this.minValue) / this.majorTicks;
				 i < j;
				 i++, m += k, n += o) {
				objMajorTickPath = this.paper
					.path('M' + m + ',' + iTop + 'L' + m + ',' + iBottom)
					.attr(objMajorTicksStyle);

				objMajorTicks.push(objMajorTickPath);

				objText = this.paper
					.text(m, iTextPos, n)
					.attr(objTextStyle);

				objLabels.push(objText);

				if (this.minorTicks > 0) {
					for (var p = 0,
							 q = this.minorTicks - 1,
							 r = k / this.minorTicks,
							 s = m + r;
						 p < q && i < j - 1;
						 p++, s += r) {

						objMinorTickPath = this.paper
							.path('M' + s + ',' + iMinorTop + 'L' + s + ',' + iMinorBottom)
							.attr(objMinorTicksStyle);

						objMinorTicks.push(objMinorTickPath);
					}
				}
			}


			iTop = (this.vAlignment === 'bottom')
				? this.vMargin
				: 0;
			iBottom = (this.vAlignment === 'bottom')
				? this.height
				: this.vMargin;

			this.meter = this.paper
				.rect(this.hMargin - this.meterStrokeWidth / 2, iTop, this.meterStrokeWidth, this.height - this.vMargin)
				.attr(objMeterStyle);

			this.majorTicks = objMajorTicks;
			this.minorTicks = objMinorTicks;
		},

		/**
		 * This function is called during instantiation, and draws a vertical meter
		 * @private
		 */
		_drawVerticalMeter: function () {
			var objMajorTicksStyle = {
					'stroke-width': this.majorTicksStrokeWidth,
					stroke:         this.stepColor
				},
				objMinorTicksStyle = {
					'stroke-width': this.minorTicksStrokeWidth,
					stroke:         this.stepColor
				},
				objTextStyle = {
					'text-anchor': (this.hAlignment === 'right')
						? 'end'
						: 'start',
					'font-size':   this.fontSize,
					fill:          this.fontColor
				},
				objMeterStyle = {
					fill:           this.meterColor,
					height:         this.meterStrokeWidth,
					opacity:        this.meterOpacity,
					'stroke-width': 0
				},
				iBasePos = (this.hAlignment === 'right')
					? this.width - this.hMargin
					: 0 + this.hMargin,
				iTop = this.vMargin,
				iBottom = this.height - this.vMargin,
				iHeight = this.height - this.vMargin * 2,
				objMajorTicks = this.paper.set(), // create Raphael set for major ticks
				objMinorTicks = this.paper.set(), // create Raphael set for minor ticks
				objLabels = this.paper.set(), // create Raphael set for major tick labels
				iLeft = (this.hAlignment === 'right')
					? this.width - this.majorTicksLength - this.hMargin
					: 0 + this.hMargin,
				iMinorLeft = (this.hAlignment === 'right')
					? this.width - this.minorTicksLength - this.hMargin
					: 0 + this.hMargin,
				iMinorRight = (this.hAlignment === 'right')
					? this.width - this.hMargin
					: this.minorTicksLength + this.hMargin,
				iRight = (this.hAlignment === 'right')
					? this.width - this.hMargin
					: this.majorTicksLength + this.hMargin,
				iTextPos = (this.hAlignment === 'right')
					? iLeft
					: this.hMargin + this.majorTicksLength,
				objMajorTickPath,
				objMinorTickPath,
				objText;

			this.basePath = this.paper
				.path('M' + iBasePos + ',' + iTop + 'L' + iBasePos + ',' + iBottom)
				.attr(objMajorTicksStyle);

			for (var i = 0,
					 j = this.majorTicks + 1,
					 k = iHeight / this.majorTicks,
					 m = iTop,
					 n = this.maxValue,
					 o = (this.maxValue - this.minValue) / this.majorTicks;
				 i < j;
				 i++, m += k, n -= o) {
				objMajorTickPath = this.paper
					.path('M' + iLeft + ',' + m + 'L' + iRight + ',' + m)
					.attr(objMajorTicksStyle);

				objMajorTicks.push(objMajorTickPath);

				objText = this.paper
					.text(iTextPos, m, n)
					.attr(objTextStyle);

				objLabels.push(objText);

				if (this.minorTicks > 0) {
					for (var p = 0,
							 q = this.minorTicks - 1,
							 r = k / this.minorTicks,
							 s = m + r;
						 p < q && i < j - 1;
						 p++, s += r) {
						objMinorTickPath = this.paper
							.path('M' + iMinorLeft + ',' + s + 'L' + iMinorRight + ',' + s)
							.attr(objMinorTicksStyle);

						objMinorTicks.push(objMinorTickPath);
					}
				}
			}

			iLeft = (this.hAlignment === 'right')
				? this.hMargin
				: this.hMargin;
			iRight = (this.hAlignment === 'right')
				? this.width - this.hMargin
				: this.width - this.hMargin;

			this.meter = this.paper
				.rect(iLeft, this.height - this.vMargin - this.meterStrokeWidth / 2, iRight - iLeft, this.meterStrokeWidth)
				.attr(objMeterStyle);

			this.majorTicks = objMajorTicks;
			this.minorTicks = objMinorTicks;
		},

		/**
		 * This function sets or gets the value of the meter
		 * @param value
		 * @return {Number}
		 */
		val: function (value) {
			var iLeft, iRight, iTop, iBottom, iWidth, iHeight, iPos, szPath;

			if (value === undefined) {
				return this.value;
			}

			if (!$.isNumeric(value)) {
				return false;
			}

			// make sure value is within range
			value = Math.min(value, this.maxValue);
			value = Math.max(value, this.minValue);

			this.value = value;

			if (this.orientation === 'horizontal') {
				iLeft = this.hMargin + this.meterStrokeWidth / 2;
				iRight = this.width - this.hMargin - this.meterStrokeWidth / 2;
				iWidth = this.width - this.hMargin * 2;

				iPos = ((value - this.minValue) / (this.maxValue - this.minValue) * iWidth) + this.hMargin - this.meterStrokeWidth / 2;

				this.meter.animate({x: iPos}, this.speed, this.easing);
			} else {
				iTop = this.vMargin + this.meterStrokeWidth / 2;
				iBottom = this.height - this.vMargin - this.meterStrokeWidth / 2;
				iHeight = this.height - this.vMargin * 2;

				iPos = ((value - this.minValue) / (this.maxValue - this.minValue) * iHeight) + this.vMargin - this.meterStrokeWidth / 2;

				iPos = iBottom - iPos + this.meterStrokeWidth / 2;

				this.meter.animate({y: iPos}, this.speed, this.easing);
			}
		},

		/**
		 * For demo purposes
		 * @return {*}
		 */
		randomize: function () {
			var iMin = this.minValue,
				iMax = this.maxValue,
				iRand = Math.floor(Math.random() * (iMax - iMin + 1) + iMin);
			this.val(iRand);
			return this;
		}

	}; // End SVGMeter.prototype

})(jQuery);



