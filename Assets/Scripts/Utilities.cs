using System;
using UnityEngine;

public static class Utilities
{
    public static Component CopyComponent(Component original, GameObject destination)
    {
        System.Type type = original.GetType();
        Component copy = destination.AddComponent(type);
        // Copied fields can be restricted with BindingFlags
        System.Reflection.FieldInfo[] fields = type.GetFields(); 
        foreach (System.Reflection.FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }
        return copy;
    }

    public static void CopyColliderProperties(Collider source, Collider destination){
         Type t = source.GetType();

         if(t == typeof(BoxCollider)){
             CopyBoxColliderProperties(source as BoxCollider, destination as BoxCollider);
         }
         else if(t == typeof(SphereCollider)){
             CopySphereColliderProperties(source as SphereCollider, destination as SphereCollider);
         }
         else if(t == typeof(MeshCollider)){
             CopyMeshColliderProperties(source as MeshCollider, destination as MeshCollider);
             destination.transform.localScale = source.transform.localScale * 0.95f;
         }
         else if(t == typeof(CapsuleCollider)){
             CopyCapsuleColliderProperties(source as CapsuleCollider, destination as CapsuleCollider);
         }
         else{
             Debug.Log("POOP");
         }
    }

    public static void CopyBoxColliderProperties(BoxCollider s, BoxCollider d){
        d.isTrigger = s.isTrigger;
        d.material = s.material;
        d.center = s.center;
        d.size = s.size * 0.95f;
    }

    public static void CopySphereColliderProperties(SphereCollider s, SphereCollider d){
        d.isTrigger = s.isTrigger;
        d.material = s.material;
        d.center = s.center;
        d.radius = s.radius * 0.95f;
    }

    public static void CopyCapsuleColliderProperties(CapsuleCollider s, CapsuleCollider d){
        d.isTrigger = s.isTrigger;
        d.material = s.material;
        d.center = s.center;
        d.radius = s.radius * 0.95f;
        d.height = s.height * 0.95f;
        d.direction = s.direction;
    }

    public static void CopyMeshColliderProperties(MeshCollider s, MeshCollider d){
        d.convex = s.convex;
        d.isTrigger = s.isTrigger;
        d.cookingOptions = s.cookingOptions;
        d.material = s.material;
        d.sharedMesh = s.sharedMesh;
    }
}
