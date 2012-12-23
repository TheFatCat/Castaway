var animationdelay = 0.2;
var timer = 0.0;
var tilesizeX = 300.0;
var tilesizeY = 60.0;
var offsetX = 0.0;
var offsetY = 0.0;


function Update () {
	timer += Time.deltaTime;
	if(timer >= animationdelay){
		timer = 0.0;
		//add one step
		offsetX+=tilesizeX;
		if(offsetX >= 1.0){
			//we exceeded bounds, reset and move down
			offsetX = 0;
			offsetY-=tilesizeY;
		}
	}
	renderer.material.mainTextureOffset = Vector2(offsetX,offsetY);

}