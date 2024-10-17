using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public Transform[] waypoints; // Daftar waypoints yang akan diikuti oleh player
    public float moveSpeed = 2f; // Kecepatan gerakan player
    public float scaleChangeSpeed = 0.5f; // Kecepatan perubahan skala untuk vertikal
    public Vector3 minScale = new Vector3(0.5f, 0.5f, 1f);  // Ukuran minimum player
    public Vector3 maxScale = new Vector3(2f, 2f, 1f);  // Ukuran maksimum player
    public float reachThreshold = 0.1f;  // Jarak untuk menentukan apakah player telah mencapai waypoint

    private int currentWaypointIndex = 0; // Index waypoint saat ini
    private Vector3 previousPosition;  // Posisi sebelumnya untuk mengecek arah gerakan vertikal

    void Start()
    {
        if (waypoints.Length > 0)
        {
            previousPosition = transform.position;
        }
    }

    void Update()
    {
        if (waypoints.Length == 0)
        {
            return; // Tidak ada waypoints, tidak melakukan apa-apa
        }

        // Mendapatkan posisi waypoint saat ini
        Transform targetWaypoint = waypoints[currentWaypointIndex];

        // Menghitung arah dari posisi player ke waypoint
        Vector3 direction = targetWaypoint.position - transform.position;
        direction.Normalize(); // Menormalkan vektor agar tetap pada arah yang benar

        // Memindahkan player menuju waypoint
        transform.position += direction * moveSpeed * Time.deltaTime;

        // Mengecek apakah player sudah mencapai waypoint
        if (Vector3.Distance(transform.position, targetWaypoint.position) < reachThreshold)
        {
            // Berpindah ke waypoint berikutnya
            currentWaypointIndex++;
            
            // Jika sudah mencapai waypoint terakhir, kembali ke waypoint pertama
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0; // Reset ke waypoint pertama (looping)
            }
        }

        // Mengubah ukuran player secara bertahap jika bergerak secara vertikal
        AdjustScaleBasedOnMovement();
    }

    // Fungsi untuk mengubah ukuran berdasarkan arah vertikal dari pergerakan player
    void AdjustScaleBasedOnMovement()
    {
        // Mengecek apakah player bergerak ke atas atau ke bawah dengan membandingkan posisi sebelumnya
        float verticalMovement = transform.position.y - previousPosition.y;

        if (verticalMovement > 0) // Jika bergerak ke atas, ukuran mengecil
        {
            ChangePlayerScale(-scaleChangeSpeed);
        }
        else if (verticalMovement < 0) // Jika bergerak ke bawah, ukuran membesar
        {
            ChangePlayerScale(scaleChangeSpeed);
        }

        // Menyimpan posisi saat ini sebagai posisi sebelumnya untuk frame berikutnya
        previousPosition = transform.position;
    }

    // Fungsi untuk mengubah ukuran player secara bertahap
    void ChangePlayerScale(float scaleAmount)
    {
        // Menghitung skala baru
        Vector3 newScale = transform.localScale + new Vector3(scaleAmount, scaleAmount, 0f) * Time.deltaTime;

        // Membatasi skala agar tetap di dalam range minimum dan maksimum
        newScale = new Vector3(
            Mathf.Clamp(newScale.x, minScale.x, maxScale.x),
            Mathf.Clamp(newScale.y, minScale.y, maxScale.y),
            newScale.z
        );

        // Set skala baru ke player
        transform.localScale = newScale;
    }
}
