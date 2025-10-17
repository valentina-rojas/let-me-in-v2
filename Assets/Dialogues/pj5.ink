-> introduccion

=== introduccion ===
Hola, ¡buen día!
+ [¿De dónde viene?]
    -> procedencia
+ [¿Estuvo en contacto con otras personas?]
    -> contacto

=== procedencia ===
De mi casa, no salí para nada más que buscar comida.
+ [¿Presentó síntomas como picazón o sarpullido?]
    -> sintomas
+ [¿Qué pasó con esas marcas en el cuello?]
    -> alergia

=== contacto ===
No, vivo sola con mis gatos, y tampoco tengo amigos.
+ [¿Qué pasó con sus gatos?]
    -> gatos
+ [¿Presentó síntomas como picazón o sarpullido?]
    -> sintomas

=== sintomas ===
Nada de momento.
+ [¿Entonces por qué tiene esas marcas en el cuello?]
    -> alergia
+ [¿Segura de que no estuvo en contacto con nadie?]
    Totalmente segura… creo.
    -> END

=== alergia ===
Es mi alergia, estos cambios de clima me están matando…
+ [¿Vino hasta acá sola?]
    Claro, sola como siempre.
    -> END
+ [¿Y los gatos?]
    ¿Eh? ¿Qué gatos...? Ah, sí, eso... los dejé con mi tía.
    -> END

=== gatos ===
¿Eh? ¿Qué gatos...? Ah, sí, eso... los dejé con mi tía.
+ [Pensé que vivía sola.]
    Eh... sí, sola, pero... a veces mi tía me da una mano. Cuando no está enferma.
    -> END

=== respuestaIngreso ===
¡Muchas gracias!
-> END

=== respuestaRechazo ===
Si me enfermo y me muero, bancate el cargo de conciencia...
-> END
