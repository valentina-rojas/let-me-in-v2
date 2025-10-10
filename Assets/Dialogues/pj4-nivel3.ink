-> introduccion

=== introduccion ===
¡Ojo con el techo, que el peinado es frágil! ¡Y caro!

+ [¿Hola...? ¿Hay alguien ahí?]
    -> saludo
+ [¿No lo veía... Y ese peinado?]
    -> peinado
+ [¿Qué trae para aportar al búnker?]
    -> aporte
-> END


=== saludo ===
Claro que sí. Soy Mauro. Estoy acá abajo. ¡Siga el jopo!

+ [Encantado, Mauro. Vamos a verificar su estado.]
    —No se preocupe, estoy sano y listo para colaborar.
    -> END
+ [Está bien, pero necesito hacer unas preguntas.]
    —Adelante, dispará. Pero cuidado con el jopo.
    -> END
-> END


=== peinado ===
Es mi orgullo. ¿Sabía que me salvó de un murciélago volador una vez? Se enredó y no pudo salir.

+ [Vaya, impresionante...]
    —Gracias, no todos los días un peinado salva vidas.
    -> END
+ [Deberíamos revisar su salud antes de dejarlo entrar.]
    —No hace falta, estoy sano y estilizado.
    -> END
-> END


=== aporte ===
Altura moral y presencia estética. Si hay que buscar cosas debajo de estanterías o colarse por ductos, yo soy el hombre. Pequeño, pero útil.

+ [Perfecto, necesitamos gente así.]
    —Me alegra ser de ayuda, aunque no ocupo mucho espacio.
    -> END
+ [Solo puedo dejar entrar a quienes cumplan las normas de seguridad.]
    —Tranquilo, igual voy a encontrar algún ducto por el cual ingresar.
    -> END
+ [Está bien, pero vigilemos que no dañe nada.]
    —No se preocupe, tengo cuidado.
    -> END
-> END


=== sintomas ===
Nada. Este jopo no se cae ni con viento viral. Estoy sano, estilizado y con movilidad reducida en ascensores.

+ [Bien, entonces puede entrar.]
    —Gracias, como puede ver no voy a ocupar mucho espacio.
    -> END
+ [Aún así necesito verificar su salud.]
    —No pasa nada, ya voy a encontrar algún ducto por el cual ingresar.
    -> END
-> END


=== respuestaIngreso ===
Gracias, como podés ver no voy a ocupar mucho espacio.
-> END

=== respuestaRechazo ===
No pasa nada, ya voy a encontrar algún ducto por el cual ingresar.
-> END
