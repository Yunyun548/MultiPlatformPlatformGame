$(function(){

	var opt = {
		wsContainer : $(".block-factory"),
		compContainer : $(".component-container"),
		compPropContainer : $(".comp-property-container"),

		blockSideSize : 3, // u
		componentSideSize : 150 //px

	};

	var init = function(){
		generateEmptyBlock();
		initAvailableComponents();
		initBtn();
	};

	var initAvailableComponents = function(){
		var components = opt.compContainer.find("li");

		components.click(function(){
						selectComponent($(this));
					});
	};

	var initBtn = function(){
		$("#dl-block").click(function(){
			var components = opt.wsContainer.find(".comp");
			var block = {
				name :"novo block",
				matrix : []
			}
			$.each(components, function(i,e){
				block.matrix.push($(e).attr("data-comp-id"));
			});
			console.log(block);
		});	
	}

	var selectComponent = function(el){
		var selClass = "selected";

		opt.compPropContainer.empty();

		if (el.hasClass(selClass)) 
			el.removeClass(selClass);
		else{
			opt.compContainer.find("li").removeClass(selClass);
			el.addClass(selClass);

			var propertiesEl = el.find(".comp-property").clone();

			opt.compPropContainer.append(propertiesEl);
		}
	};

	var generateEmptyBlock = function(){
		// +10 pour le margin
		var blockFactorySideSize = (opt.componentSideSize + 10) * opt.blockSideSize;

		$(".block-factory")
					.css("width", blockFactorySideSize)
					.css("height", blockFactorySideSize)


		for (var i = opt.blockSideSize - 1; i >= 0; i--) {
			for (var j = opt.blockSideSize - 1; j >= 0; j--) {
				$("<div>").addClass("comp")
					.css("width", opt.componentSideSize)
					.css("height", opt.componentSideSize)
					.click(function(){
						attachComponent($(this));
					})
					.appendTo(opt.wsContainer);
			};
		};
	};

	var attachComponent = function(el){
		var selectedComponent = opt.compContainer.find(".selected");
		var imgEl = selectedComponent.find("img");
		el.find("img").remove();

		el.append($("<img>").prop("src", imgEl.prop("src")));
		el.attr("data-comp-id", selectedComponent.attr("data-comp-id"));

		if (isBlockValid()) {
			$("#dl-block").prop("disabled", false);
		};

	};

	var isBlockValid = function(){
		var result = true;
		var components = opt.wsContainer.find(".comp");
		
		if(!$(".work-space h5 .form-control").val())
			result = false;

		$.each(components, function(i,e){
			 if (!$(e).attr("data-comp-id")) {
				result = false;
			}
		});
		return result;
	};


	init();
});