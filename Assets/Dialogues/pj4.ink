-> introduccion

=== introduccion ===
Hola, necesito ayuda.
+ [¿De dónde viene?]
    -> procedencia
+ [¿Qué tipo de ayuda necesita?]
    -> tipoAyuda

=== procedencia ===
Vengo deambulando entre ciudades, me quedé varado en un pueblo donde fui a tocar cuando arrancó la pandemia.
+ [¿Estuvo en contacto con otra gente?]
    -> contacto
+ [¿Qué tipo de música toca?]
    -> musica

=== tipoAyuda ===
Necesito un lugar donde descansar... y quizás alguien que aprecie mi talento, que parece inmune al reconocimiento.
+ [¿Estuvo en contacto con otras personas?]
    -> contacto
+ [¿Presentó síntomas como picazón o sarpullido?]
    -> sintomas

=== contacto ===
No, si no fue a verme nadie, tengo una bronca.
+ [Se ve que lo salvó su falta de talento. ¿Compartió instrumentos con otros músicos?]
    -> instrumentos
+ [¿Presentó síntomas como picazón o sarpullido?]
    -> sintomas


=== musica ===
Rock, aunque a veces me tiran cosas más folk... especialmente tomates.
+ [¿Estuvo en contacto con otra gente?]
    -> contacto
+ [¿Presentó síntomas como picazón o sarpullido?]
    -> sintomas

=== sintomas ===
No, nada raro en la piel.
+ [Bueno, parece que su falta de talento lo inmunizó. ¿Compartió instrumentos?]
    -> instrumentos
+ [¿Estuvo en algún lugar concurrido?]
    -> concurrido

=== instrumentos ===
Sí, pedí prestada la guitarra, no me alcanza para comprarme una.
+ [¿Estuvo en algún lugar concurrido? Aparte de su recital, donde claramente no lo fue a ver nadie.]
    -> concurrido

=== concurrido ===
No, solo interactué con mi banda, que me abandonó apenas se anunció lo del virus.
    -> END

=== respuestaIngreso ===
¡Gracias! Prometo alegrarlos con unos temas... tranquilos, tengo buen repertorio para pandemias.
-> END

=== respuestaRechazo ===
Bueno, voy a seguir buscando a mi grupo... si no se desarmó por completo.
-> END
