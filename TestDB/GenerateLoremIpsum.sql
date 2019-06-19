﻿CREATE FUNCTION GenerateLoremIpsum ( @n INT = 0 )
RETURNS VARCHAR(MAX)
AS 
    BEGIN
        DECLARE @Output VARCHAR(MAX)
        DECLARE @lipsumText VARCHAR(MAX) = --Lorem Ipsum Text from www.lipsum.com, believed to be public domain
            'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris lacus arcu, blandit non semper elementum, fringilla sodales est. Ut porttitor blandit sapien pellentesque pretium. Donec ut diam sed urna venenatis hendrerit. Nulla eros arcu, mattis vitae congue cursus, tincidunt sed turpis. Curabitur non enim diam, eget elementum dolor. Vivamus enim tortor, tempor at vehicula ac, malesuada id est. Praesent at nibh eget metus dapibus dapibus. Donec arcu orci, sagittis eu interdum vitae, facilisis quis nibh.

Mauris luctus molestie velit, at vestibulum magna cursus sit amet. Nulla in accumsan libero. Donec sed sem lectus. Mauris congue sapien et diam euismod vitae scelerisque diam tincidunt. Praesent a justo enim, vitae venenatis dolor. Donec in tortor at magna dapibus suscipit sit amet a libero. Vivamus porttitor rhoncus tellus, at luctus nisl semper bibendum. Fusce eget accumsan orci. Donec eleifend mattis imperdiet. Pellentesque eget semper ipsum.

Morbi auctor eleifend tortor. Suspendisse condimentum tincidunt pharetra. Sed elementum leo nulla. Praesent a ligula nisi. Etiam ac lacus nibh. Nulla sit amet sapien mauris, sit amet pulvinar tellus. Donec sit amet turpis lorem. Mauris ut eleifend est. Donec ultrices ullamcorper nibh nec pharetra. Suspendisse vel nibh elementum lacus molestie cursus. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Aliquam in ligula orci. Nam adipiscing, erat non rutrum tincidunt, ante nunc consequat sem, et gravida felis augue vel libero. Duis interdum, odio vitae lobortis venenatis, magna nisl consectetur mi, sed aliquet nisl sem ut massa. Maecenas ut turpis metus, convallis cursus justo.

Phasellus rhoncus, magna sit amet ullamcorper adipiscing, enim mauris dictum est, eget eleifend ipsum lorem molestie sapien. Integer erat velit, fermentum nec egestas sed, lobortis et diam. Nam leo elit, malesuada ac commodo quis, varius et nulla. Etiam fringilla sollicitudin hendrerit. In hac habitasse platea dictumst. Duis hendrerit laoreet elit eu luctus. Integer at arcu lacus, in ullamcorper urna. Aenean vehicula ante in dui cursus vitae suscipit magna viverra.

Nam non diam a nisl commodo posuere id sed lorem. Donec tempor pretium lobortis. Fusce condimentum lorem eget arcu semper quis accumsan nibh interdum. Nullam purus diam, tempus id commodo ac, lacinia nec purus. Pellentesque risus ipsum, sollicitudin a accumsan eget, faucibus eu lorem. Nunc pretium, risus id gravida dapibus, eros neque blandit ipsum, sit amet rhoncus massa felis eu enim. Nunc nibh nulla, sodales quis semper sit amet, bibendum nec lacus. Maecenas massa libero, tempor eu tincidunt et, bibendum sed ante. In lobortis velit nec diam varius et tempus arcu varius. Duis convallis aliquam laoreet. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Duis dapibus pretium erat, gravida scelerisque lacus lobortis vel. Etiam velit quam, pharetra non elementum ac, dapibus at sapien.

Vivamus ultricies, libero in semper scelerisque, metus mauris vehicula ligula, eget posuere massa magna a felis. Sed vulputate massa at orci vulputate vitae dignissim libero facilisis. Integer tincidunt lacinia turpis id sodales. Donec interdum facilisis elementum. Maecenas faucibus vehicula tellus et viverra. Cras ac ipsum ac leo malesuada laoreet nec in nibh. Ut eget augue et elit tincidunt consequat at non purus. Duis pretium metus eu nisi euismod aliquam. Integer id urna vitae tellus vehicula varius ut eu nisl. Sed non nibh leo, sit amet sodales ipsum. Donec tortor magna, elementum ac cursus consequat, egestas nec odio. Curabitur volutpat risus ac mauris tempor euismod.

Aliquam tempus adipiscing diam. Cras laoreet ante nec metus egestas non hendrerit dui iaculis. Cras a arcu purus, ac facilisis turpis. Aliquam erat volutpat. Integer in ligula purus, id congue nunc. Mauris quis elementum arcu. Ut tincidunt lacus lectus, at dignissim nibh. Suspendisse potenti. Sed luctus vehicula purus eu imperdiet. Proin bibendum ullamcorper arcu at sagittis. Phasellus neque nunc, imperdiet eu malesuada eget, pharetra vel enim.

Proin in ligula eget ipsum sodales auctor. Phasellus sed nulla justo. Integer ipsum nisl, condimentum eu pellentesque sit amet, mattis non odio. In eget est augue, a luctus tortor. Nunc rhoncus quam non magna rutrum cursus. Morbi ornare rhoncus tincidunt. Nullam scelerisque nunc mi. Etiam in tempor ipsum. Nulla in felis est, sed feugiat ligula. Praesent feugiat mauris ultrices ligula molestie bibendum dictum lorem faucibus. Sed laoreet mauris vestibulum justo sodales sit amet pellentesque ligula ultrices. Curabitur euismod, quam et rhoncus vulputate, lacus sem vulputate sapien, et auctor lacus sapien non augue. Pellentesque id urna mauris, vel sollicitudin arcu. Nunc ligula libero, laoreet in placerat id, tristique a risus.

Aliquam erat volutpat. Suspendisse in ultrices leo. Pellentesque eleifend justo quis elit scelerisque cursus. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. In commodo pulvinar sem, ut volutpat enim dapibus eget. Donec arcu neque, lacinia non ultricies nec, tristique in erat. Duis a mauris ac elit eleifend posuere. Morbi sit amet lorem et diam blandit placerat. Nullam sed sem ac odio fermentum malesuada. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos.

Vivamus a lacus lorem. Ut purus tellus, aliquet a dapibus quis, semper quis neque. Pellentesque tincidunt turpis eget orci scelerisque semper. Vivamus eget tellus eget libero ultricies porttitor eu eget lectus. Quisque ut elit ac magna faucibus mollis sed dapibus diam. In ut nibh eget erat fermentum blandit sed sed massa. Curabitur et magna a ipsum condimentum suscipit nec vestibulum mauris. Integer ut elit at quam posuere eleifend. Proin mollis placerat nunc vitae tincidunt. Pellentesque vitae enim sed arcu aliquet laoreet id nec urna. Nullam nulla massa, bibendum at commodo at, lobortis quis tellus. Mauris enim libero, hendrerit vitae vestibulum at, pharetra sed diam. Duis mauris nibh, tincidunt ut ornare eget, suscipit at nulla. Mauris quis diam vel eros commodo aliquam.

Donec et faucibus nisl. Curabitur convallis turpis sed dolor molestie eget placerat nunc facilisis. Donec mauris diam, pulvinar non pellentesque ac, cursus a eros. Nullam nulla nunc, feugiat eget congue nec, dignissim ut magna. Aliquam id nulla tellus. Morbi imperdiet fringilla lacus a facilisis. Sed faucibus urna id turpis sollicitudin consectetur. In eget iaculis libero. Sed quis tellus nisi, nec posuere enim. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Nam suscipit orci in enim lobortis viverra. Integer vulputate scelerisque mauris, in porta urna imperdiet sed. Vivamus libero tellus, dictum sit amet condimentum non, hendrerit in diam. Nam ultrices ultrices leo, in adipiscing ante posuere sit amet. Integer gravida nunc et est mollis in suscipit dolor scelerisque. Fusce ultrices facilisis tortor ac rhoncus.

Nunc sed orci justo, et egestas augue. Nulla id elit scelerisque justo aliquet ultricies. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Donec pretium sagittis mi, a blandit felis auctor sed. Morbi aliquam elit ac dui iaculis imperdiet. Integer a lectus quam, ut pellentesque elit. Suspendisse eget orci sed lectus commodo venenatis. Fusce interdum risus ac turpis tincidunt rhoncus. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; In malesuada nulla ut ante luctus facilisis. Vestibulum ac odio ante. Ut odio erat, placerat at condimentum et, adipiscing eu sapien. Ut nibh magna, eleifend eget rutrum vitae, mattis id sapien. Pellentesque quis ligula at turpis dignissim rutrum. Aenean nibh massa, varius in mattis quis, molestie eget massa.

Curabitur quam nisl, eleifend a vestibulum at, porta a odio. Phasellus tristique varius placerat. Cras nec iaculis nisl. Vestibulum ac magna dui. Nullam a nunc sem, a ullamcorper tellus. Donec tristique, odio eget vehicula hendrerit, nunc nibh viverra tortor, at consectetur arcu magna in justo. Vestibulum laoreet faucibus est iaculis dictum. Integer at augue magna, non auctor lacus.

Maecenas urna mi, consequat sed fermentum sit amet, eleifend non lorem. Cras a tortor ac nisl luctus lacinia. Donec viverra, augue vitae mollis vulputate, nisl nibh interdum enim, id porta velit ipsum volutpat erat. Donec euismod quam non nisi sagittis commodo condimentum mauris consequat. Phasellus sodales, turpis sit amet laoreet consectetur, magna risus tincidunt libero, eu sagittis tortor tortor id mi. Nunc id enim nec tortor dictum rutrum. Vivamus rhoncus rhoncus ipsum, non consectetur mi tempus nec. Morbi eu ligula sit amet neque facilisis posuere. Nam mattis, tortor at feugiat vestibulum, ante justo ornare tortor, et laoreet arcu felis in purus. Vestibulum consequat iaculis libero ut euismod.

Quisque eget felis in est interdum tempus ut vitae sem. Nunc tristique mi ac lectus malesuada sit amet sodales dui bibendum. Donec accumsan, eros ac interdum interdum, mi felis consequat justo, nec euismod metus eros eget libero. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Donec auctor, enim id fringilla cursus, elit purus interdum ligula, sed blandit massa enim vitae eros. Aliquam tristique magna non odio condimentum dignissim. In pellentesque purus elementum lectus auctor in tempus justo vestibulum. Aenean ac tellus non nunc pulvinar imperdiet eget vel purus. In non ipsum urna, et semper arcu. Integer sit amet malesuada orci. Sed laoreet tempor erat sed consequat. Nullam iaculis adipiscing leo in hendrerit. Nullam pellentesque suscipit sapien, et sagittis mi auctor et.

Nullam vel hendrerit nunc. Cras et justo quis nunc cursus porttitor semper vulputate nisl. Curabitur vel aliquam dolor. Etiam suscipit lobortis accumsan. Mauris non velit at urna rutrum accumsan. Cras mi tellus, varius ac accumsan eu, auctor id justo. Maecenas vehicula congue tortor, vel iaculis sapien consequat vitae. Nulla id ligula ac ipsum lacinia volutpat. Nullam convallis erat et purus suscipit adipiscing. Praesent massa neque, pharetra non scelerisque non, accumsan non elit. Duis ut lacus lacus, ac sollicitudin purus. Quisque in ante velit. Nam tempus ligula id ipsum placerat eu feugiat nulla ullamcorper. Etiam venenatis hendrerit erat a luctus. Pellentesque pulvinar ipsum eget velit luctus imperdiet.

In volutpat neque gravida leo mollis nec venenatis purus accumsan. Nam iaculis mi at nulla imperdiet pulvinar. Duis eget velit risus. Suspendisse imperdiet, odio quis accumsan ullamcorper, libero libero eleifend leo, in ullamcorper tortor libero non diam. Proin ut felis venenatis justo pulvinar mattis. Nulla facilisi. Mauris at mi metus. Ut consectetur ligula eget felis euismod vel luctus nulla pretium. Etiam euismod nisi ac nisi fringilla sed condimentum lectus gravida. Donec ligula arcu, interdum vel volutpat vitae, dignissim eget eros. Etiam nec massa eros. Donec auctor imperdiet facilisis.

Integer sed vulputate metus. Vivamus nec dolor auctor ipsum suscipit facilisis vitae in orci. Ut commodo viverra dui, nec egestas nisi mollis vitae. Morbi mi massa, suscipit eget malesuada non, blandit in mi. Donec condimentum justo nec nisl sagittis semper. Vestibulum ornare nunc tincidunt velit congue gravida. Cras convallis semper tellus, vitae luctus libero viverra nec. Morbi fermentum luctus varius. Cras odio purus, tempor quis varius quis, ultrices vel nibh.

Suspendisse euismod dictum urna, volutpat iaculis risus volutpat non. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Integer ornare purus sodales diam commodo convallis. Cras scelerisque scelerisque faucibus. Donec est lacus, semper rutrum tristique id, vulputate sit amet arcu. Sed ornare lorem ut velit luctus ut posuere diam sollicitudin. Donec facilisis, dui cursus tincidunt ultrices, mi mi varius leo, et volutpat velit quam in est. Vivamus consectetur diam non augue mollis sed bibendum ligula faucibus. Donec nec quam ipsum. Praesent ultricies, risus quis sollicitudin commodo, diam odio pharetra nisi, eu iaculis turpis dolor et ante. Curabitur massa ante, rhoncus in tempus sit amet, elementum vel turpis. Fusce quis ligula et tortor imperdiet interdum. Suspendisse potenti. Sed congue ante vitae tellus convallis egestas sit amet vel arcu.

Ut et sapien nisi. Etiam sed ipsum sed urna porta venenatis. Sed ultrices aliquam congue. Aliquam nulla leo, mattis sit amet ornare et, sodales sit amet velit. Nullam vitae metus quis purus eleifend condimentum. In libero diam, pretium non placerat ut, vestibulum at magna. Suspendisse arcu lectus, sagittis ut malesuada eu, scelerisque id nulla.

Maecenas lacinia dolor vel nunc dictum iaculis. Sed sed augue nibh, eget gravida lorem. Aliquam pulvinar arcu eu orci adipiscing eget pulvinar mi tincidunt. Etiam eu ipsum lorem. Nam eget lacus vel orci tincidunt consequat a quis erat. Nulla sit amet lacus ac nibh facilisis porta a in elit. Donec quis sem id leo gravida porttitor. Quisque condimentum tincidunt metus, non pellentesque dui ultrices ac.

In in scelerisque dui. In a neque vitae diam pulvinar sollicitudin. Sed semper pulvinar est quis rhoncus. Sed non nunc ac libero convallis consequat. Etiam vel odio nibh. Mauris sed orci nulla, faucibus posuere erat. Suspendisse molestie dui at tortor ullamcorper vulputate. Sed euismod erat a nulla dapibus ornare.

Pellentesque viverra nisl sed tellus blandit gravida. Nunc eu ante sed diam condimentum pulvinar. Duis dapibus consequat tellus, ut tristique ligula auctor at. Quisque consectetur dictum turpis, at fringilla tellus lacinia at. Phasellus viverra sollicitudin porttitor. Suspendisse potenti. Mauris mauris leo, ornare nec ullamcorper vel, dapibus eu tellus.

In lacinia ipsum ligula, nec tristique dolor. Duis sit amet eros lorem. Vestibulum velit ipsum, commodo vitae rutrum vel, adipiscing vel lorem. Sed et eros quis dui facilisis pulvinar a ac ligula. Sed augue purus, facilisis sit amet scelerisque et, bibendum sed mauris. Etiam et libero lorem. Aliquam mattis, orci at tristique vehicula, erat velit bibendum ipsum, id dignissim erat ipsum et nisi. Pellentesque pharetra neque ut dolor auctor ac lobortis mauris eleifend. Ut elit massa, congue ultricies fringilla eu, vestibulum a dui. Donec mattis neque ac nisi fringilla ut scelerisque mauris tincidunt. Nulla iaculis hendrerit est, ac dignissim lectus ornare nec. Etiam facilisis, magna sit amet congue volutpat, nulla orci lacinia eros, sit amet auctor nibh nisi non nulla.

Donec vel auctor velit. Morbi aliquet urna et arcu rhoncus malesuada. Maecenas in urna ut risus vulputate porttitor. Pellentesque non lacus quis nisi tempus fringilla. In lectus risus, consectetur eget dictum ut, vestibulum at odio. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Etiam consequat justo mollis augue blandit a tincidunt libero interdum. Integer tempus venenatis justo non sodales. Morbi tristique velit eu mi iaculis blandit.

Fusce eget ipsum vel augue iaculis congue at eget nibh. Fusce bibendum tincidunt quam eu rutrum. Fusce placerat arcu non eros adipiscing bibendum hendrerit neque eleifend. Phasellus iaculis tellus ut risus malesuada ornare. Proin non orci nec leo mattis sollicitudin molestie ac tellus. Nullam aliquam massa vitae diam tincidunt mollis. Nam et lacus diam. Aenean facilisis est sit amet tortor ultricies ut pellentesque diam iaculis.

Morbi lorem enim, rhoncus eget dapibus vel, iaculis in nunc. Nunc arcu magna, gravida a blandit venenatis, volutpat in erat. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Nulla ut ante lectus. Morbi eu metus urna, a blandit libero. Quisque sapien nisl, elementum a vulputate vel, iaculis a risus. Quisque sed tincidunt orci. Lorem ipsum dolor sit amet, consectetur adipiscing elit amet.'

        SET @Output = LEFT(@lipsumText, @n) ;
        RETURN @Output
    END