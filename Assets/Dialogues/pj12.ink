-> introduccion

=== introduccion ===
¡Buenos días!

+ [¿Estuvo en contacto con otra gente?]
    No, estoy sola con mi gato.
    -> contactoGato
+ [¿Mantuvo precauciones?]
    Estuve aislada en mi casa; mi barrio es un caos y no quiero volver ahí.
    -> precauciones
-> END


=== contactoGato ===
+ [¿Sabe que no puede ingresar con mascotas?]
    Pero es mi FAMILIA, no puedo dejarlo, ¡por favor!
    -> motivoMascota
+ [¿Tomó todas las medidas para no contagiarse?]
    Sí, sigo todas las normas de higiene y aislamiento.
    -> precauciones
-> END


=== precauciones ===
+ [¿Por qué vino entonces?]
    Quiero estar en un lugar seguro, no puedo quedarme sola en el barrio.
    -> motivoMascota
-> END


=== motivoMascota ===
+ [Lamentablemente no podemos dejar ingresar mascotas.]
    Entiendo... pero no pienso dejarlo.
-> END




=== respuestaIngreso ===
¡Muchas gracias! Walter y yo te lo agradecemos...
-> END

=== respuestaRechazo ===
Ahora solo somos Walter y yo por nuestra cuenta...
-> END
