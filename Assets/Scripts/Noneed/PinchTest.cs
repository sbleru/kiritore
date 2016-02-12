using UnityEngine;
using System.Collections;

public class PinchTest : MonoBehaviour {
	
	private enum PINCH_STATUS
	{
		NONE = -1,
		
		UP = 0,
		PINCH,
	};
	
	private PINCH_STATUS pinchStatus = PINCH_STATUS.UP;
	private PINCH_STATUS pinchStatusNext = PINCH_STATUS.NONE;
	
	private float pinchLength;
	private Vector2 center;
	
	public GameObject cube;
	public Camera cam;
	
	void Start()
	{
	}
	
	void movePoint(Vector2 po)
	{
		Vector3 nowPo = cube.transform.localPosition;
		cube.transform.localPosition = new Vector3(po.x, po.y, nowPo.z);
	}
	
	Vector2 convertCenter(Vector2 po)
	{
		float scale = cam.orthographicSize / 480f;
		po *= scale;
		Vector2 camPos = new Vector2(cam.transform.localPosition.x, cam.transform.localPosition.y);
		Vector2 org = camPos - new Vector2(320f * scale, 480f * scale);
		po += org;
		return po;
	}
	
	void Update()
	{
		switch(pinchStatus)
		{
		case PINCH_STATUS.UP:
			if(Input.touchCount > 1)
			{
				Touch touch0 = Input.GetTouch(0);
				Touch touch1 = Input.GetTouch(1);
				
				pinchLength = Vector2.Distance(touch0.position, touch1.position);
				center = (touch0.position + touch1.position) * 0.5f;
				center = convertCenter(center);
				print(touch0.position + ", " + touch1.position + ", " + center);
				movePoint(center);
				
				pinchStatusNext = PINCH_STATUS.PINCH;
			}
			break;
			
		case PINCH_STATUS.PINCH:
			if(Input.touchCount < 2)
			{
				pinchStatusNext = PINCH_STATUS.UP;
			}
			break;
		}
		
		while(pinchStatusNext != PINCH_STATUS.NONE)
		{
			pinchStatus = pinchStatusNext;
			pinchStatusNext = PINCH_STATUS.NONE;
		}
		
		switch(pinchStatus)
		{
		case PINCH_STATUS.PINCH:
			Touch touch0 = Input.GetTouch(0);
			Touch touch1 = Input.GetTouch(1);
			float nowPinchLength = Vector2.Distance(touch0.position, touch1.position);
			float scale = nowPinchLength / pinchLength;
			
			cam.GetComponent<Camera>().orthographicSize /= scale;
			pinchLength = nowPinchLength;
			
			Vector2 nowCamPos = new Vector2(cam.transform.localPosition.x, cam.transform.localPosition.y);
			Vector2 diff = center - nowCamPos;
			scale = 1.0f - scale;
			diff = diff * scale;
			
			cam.transform.localPosition -= new Vector3(diff.x, diff.y, 0);
			
			break;
		}
	}
}
