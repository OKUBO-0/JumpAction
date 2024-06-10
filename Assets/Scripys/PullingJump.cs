using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullingJump : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 clickPosition;
    [SerializeField]
    private float jumpPower = 10;
    private bool isCanJump;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        // rb = GetComponent<Rigidbody>();  //gameObjectは省略可能
        Physics.gravity = new Vector3(0, -20, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickPosition = Input.mousePosition;
        }
        if (isCanJump && Input.GetMouseButtonUp(0))
        {
            //クリックした座標と離した座標の差分を取得
            Vector3 dist = clickPosition - Input.mousePosition;
            //クリックとリリースが同じ座標ならば無視
            if (dist.sqrMagnitude == 0) { return; }
            //差分を標準化し、jumpPowerをかけ合わせた佰を移動量とする。
            rb.velocity = dist.normalized * jumpPower;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Debug.Log("衝突した");
    }

    private void OnCollisionStay(Collision collision)
    {
        // Debug.Log("衝突した");
        // isCanJump = true;

        // 衝突している点の情報が複数格納されている
        ContactPoint[] contacts = collision.contacts;

        // 0番目の衝突情報から、衛突している点の法線を取得
        Vector3 otherNormal = contacts[0].normal;

        // 上方向を示すベクトル。長さは1
        Vector3 upVector = new Vector3(0, 1, 0);

        // 上方向と法線の内積。二つのベクトルはともに長さが1なので、Cosθの結果がdotUN変数に入る。
        float dotUN = Vector3.Dot(upVector, otherNormal);

        // 内積信に逆三角関数arccosを掛けて角度を算出。それを度数法へ変換する。これで度が算出できた。
        float dotDeg = Mathf.Acos(dotUN) * Mathf.Rad2Deg;

        // ニつのベクトルがなす角度が45度より小さければ再びジャンプ可能とする。
        if(dotDeg <= 45)
        {
            isCanJump = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Debug.Log("衝突した");
        isCanJump = false;
    }
}