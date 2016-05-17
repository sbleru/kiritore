
// シーン間で共有する変数 このクラスではインスタンスは作らない
public static class SetValue  {
	public static int total_score;
	public static int stage_num;
	public static float time;

	//メンバ変数が使われる時に１度だけ呼ばれる
	static SetValue(){
		total_score = 0;
		stage_num = 1;
		time = 30;
	}

	static public void initialize(){
		total_score = 0;
		stage_num = 1;
		time = 30;
	}
}
