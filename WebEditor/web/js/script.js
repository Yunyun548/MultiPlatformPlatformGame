$(function(){

	var opt = {
		wsContainer : $(".block-factory"),
		compContainer : $(".component-container"),

		blockSideSize : 4

	};

	var init = function(){
		generateEmptyBlock();
		getAvailableComponents();
	};

	var getAvailableComponents = function(){

		postBackAvailableComponents([
			{
				name : "composant1",
				color : "red"
			},
			{
				name : "composant2",
				color :"blue"
			},
			{
				name : "composant3",
				color : "green"
			}
		]);
	};

	var getExistingBlocks = function(){

	};

	var postBackAvailableComponents = function(componentList){
		$.each(componentList, function(i,e){
			$("<li>").text(e.name)
					.attr("data-color", e.color)
					.click(function(){
						selectComponent($(this));
					})
					.appendTo(opt.compContainer);
		});
	};

	var selectComponent = function(el){
		var selClass = "selected";


		if (el.hasClass(selClass)) 
			el.removeClass(selClass);
		else{
			opt.compContainer.find("li").removeClass(selClass);
			el.addClass(selClass);
		}
	};

	var generateEmptyBlock = function(){
		for (var i = opt.blockSideSize - 1; i >= 0; i--) {
			for (var j = opt.blockSideSize - 1; j >= 0; j--) {
				$("<div>").addClass("comp")
					.text(i+ " - " + j)
					.click(function(){
						attachComponent($(this));
					})
					.appendTo(opt.wsContainer);
			};
		};
	};

	var attachComponent = function(el){
		var selectedComponent = opt.compContainer.find(".selected");

		el.css("color", selectedComponent.attr("data-color"));

	};


	init();
});