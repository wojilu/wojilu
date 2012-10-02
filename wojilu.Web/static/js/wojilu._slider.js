
// http://www.sohtanaka.com/web-design/automatic-image-slider-w-css-jquery/

define( [], function() {

    var _init = function() {

        $('.slideWrap').each( function() {

            var slide = $(this);
            var imageSum = $("img",slide).size();
            if( imageSum<=0 ) return;

            slide.wrapInner( '<div class="slideWindow" style="width:'+slide.width()+'px;height:'+slide.height()+'px;"><div class="slideReel"></div></div>' ).append('<div class="slideBar"></div> ');
            $("img",slide).show();

            var imageWidth = $(".slideWindow",slide).width();
            var imageHeight = $(".slideWindow",slide).height();
            var imageReelWidth = imageWidth * imageSum;
            $(".slideReel",slide).css({'width' : imageReelWidth});

            var getLink = function(ele) {
                var alink = ele.parent();
                var alt = ele.attr('alt');
                if( alt ) {
                    return '<a href="'+alink.attr('href')+'" class="'+alink.attr('class')+'" target="'+alink.attr('target')+'">'+alt+'</a>';
                }
                else {
                    return "";
                }
            };
            var first = $($(".slideReel img",slide)[0]);
            var txtWidth = imageWidth- imageSum*30-10;
            var barHtml='<div class="slideBarText" style="width:'+txtWidth+'px">'+getLink(first)+'</div><div class="slideBarNum">';

            $(".slideReel img",slide).each( function(index,val) {
                var pageNum = parseInt( index )+1;
                barHtml += '<a href="#" rel="'+pageNum+'">'+pageNum+'</a> ';
                $(this).width(imageWidth).height(imageHeight);
            });
            barHtml+='</div>';
            $('.slideBar',slide).html( barHtml);

            $(".slideBar",slide).show();
            $(".slideBarNum a:first",slide).addClass("active");
            
            rotate = function(){	
                var triggerID = $active.attr("rel") - 1; //Get number of times to slide
                var cimg = $($(".slideReel img",slide)[triggerID]);
                $active.parent().prev().html( getLink(cimg) );
                var slideReelPosition = triggerID * imageWidth; //Determines the distance the image reel needs to slide
         
                $(".slideBarNum a",slide).removeClass('active'); //Remove all active class
                $active.addClass('active'); //Add active class (the $active is declared in the rotateSwitch function)
                
                if( $('.slideReel a',slide).size()==0 ) return;
                
                //幻灯片动画移动效果：向左移动
                $(".slideReel",slide).animate({ 
                    left: -slideReelPosition
                }, 500 );
                // 750, 'easeInOutExpo'
                
            }; 
            
            rotateSwitch = function(){		
                play = setInterval(function(){ //Set timer - this will repeat itself every 3 seconds
                    $active = $('.slideBarNum a.active',slide).next();
                    if ( $active.length === 0) { //If slideBar reaches the end...
                        $active = $('.slideBarNum a:first',slide); //go back to first
                    }
                    rotate(); //Trigger the slideBar and slider function
                    
                }, 4000); 
            };

            rotateSwitch(); //Run function on launch
            
            //On Hover
            $(".slideReel a",slide).hover(function() {
                clearInterval(play); //Stop the rotation
            }, function() {
                rotateSwitch(); //Resume rotation
            });	
            
            //On Click
            $(".slideBarNum a",slide).click(function() {	
                $active = $(this); //Activate the clicked slideBar
                //Reset Timer
                clearInterval(play); //Stop the rotation
                rotate(); //Trigger rotation immediately
                rotateSwitch(); // Resume rotation
                return false; //Prevent browser jump to link anchor
            });	
        });

    };


    return { init : _init };

});
