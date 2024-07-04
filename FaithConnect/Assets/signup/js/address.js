const barangays = {
    "Cebu City": [
        "Adlaon", "Agsungot", "Apas", "Babag", "Bacayan", "Banilad", "Basak Pardo", "Basak San Nicolas", "Binaliw",
        "Bonbon", "Budlaan", "Bulacao", "Buot-Taup", "Busay", "Cambinocot", "Capitol Site", "Carreta", "Central",
        "Cogon Pardo", "Cogon Ramos", "Day-as", "Duljo Fatima", "Ermita", "Guba", "Guadalupe", "Hipodromo", "Inayawan",
        "Kalubihan", "Kalunasan", "Kamagayan", "Kamputhaw (Lahug)", "Kasambagan", "Kinasang-an Pardo", "Labangon", "Lahug",
        "Lorega San Miguel", "Lusaran", "Mabini", "Mabolo", "Malubog", "Mambaling", "Parian", "Pasil", "Pit-os",
        "Poblacion Pardo", "Pung-ol Sibugay", "Punta Princesa", "Quiot Pardo", "Sambag I", "Sambag II", "San Antonio",
        "San Jose", "San Nicolas Proper", "Santa Cruz", "Santo Niño", "Sirao", "Suba", "Sudlon I", "Sudlon II",
        "T. Padilla", "Tabunan", "Tagbao", "Talamban", "Taptap", "Tejero", "Tinago", "Tisa", "Toong", "Zapatera"
    ],
    "Lapu-Lapu City": [
        "Agus", "Babag", "Bankal", "Baring", "Basak", "Buaya", "Calawisan", "Canjulao", "Catarman", "Caubian",
        "Gun-ob", "Ibo", "Looc", "Mactan", "Maribago", "Marigondon", "Pajac", "Pajo", "Pangan-an", "Poblacion",
        "Pusok", "Sabang", "Santa Rosa", "Suba-basbas", "Talima", "Tingo", "Tungasan", "Punta Engaño", "San Vicente",
        "Buagsong", "Tungkil"
    ],
    "Mandaue City": [
        "Alang-alang", "Bakilid", "Banilad", "Basak", "Cabancalan", "Cambaro", "Canduman", "Casili", "Casuntingan",
        "Centro", "Cubacub", "Guizo", "Ibabao-Estancia", "Jagobiao", "Labogon", "Looc", "Maguikay", "Mantuyong",
        "Opao", "Pakna-an", "Pagsabungan", "Subangdaku", "Tabok", "Tawason", "Tingub", "Tipolo", "Umapad"
    ],
    "Talisay City": [
        "Biasong", "Bulacao", "Cadulawan", "Camp 4", "Candulawan", "Cansojong", "Dumlog", "Jaclupan", "Lagtang",
        "Lawaan I", "Lawaan II", "Lawaan III", "Linao", "Maghaway", "Manipis", "Mohon", "Poblacion", "Pooc",
        "San Isidro", "San Roque", "Tabunok", "Tangke", "Tapul", "Tungkalan"
    ],
    "Toledo City": [
        "Bagakay", "Bato", "Bulongan", "Cabitoonan", "Calongcalong", "Cambang-ug", "Canlumampao", "Cantabaco",
        "Carmen", "Daanglungsod", "Don Andres Soriano (Lutopan)", "Dumlog", "General Climaco (Casoy)", "Ilihan",
        "Landahan", "Loay", "Lutopan", "Magdugo", "Malubog", "Matab-ang", "Media Once", "Poblacion", "Poog",
        "Putingbato", "Sam-ang", "Sangi", "Sapangdaku", "Sawang", "Subayon", "Talavera", "Tubod", "Tungkop",
        "Tubod-Dugoan", "Ibo"
    ],
    "Naga City": [
        "Alpaco", "Balirong", "Cantao-an", "Central Poblacion", "Colon", "East Poblacion", "Inayagan", "Inoburan",
        "Jaguimit", "Lanas", "Langtad", "Lutac", "Mainit", "Mayana", "Naalad", "North Poblacion", "Pangdan",
        "Patag", "South Poblacion", "Tagjaguimit", "Tangke", "Tinaan", "Tuyan", "Uling"
    ],
    "Carcar City": [
        "Bolinawan", "Buenavista", "Calidngan", "Can-asujan", "Guadalupe", "Liburon", "Napo", "Ocaña", "Perrelos",
        "Poblacion I", "Poblacion II", "Poblacion III", "Tuyan", "Valencia", "Valladolid"
    ],
    "Bogo City": [
        "Anonang Sur", "Binabag", "Bungtod", "Cayang", "Cogon", "Dakit", "Don Pedro Rodriguez", "Gairan",
        "Guadalupe", "La Paz", "Lapaz Viejo", "Libertad", "Lourdes", "Malingin", "Marangog", "Nailon", "Polambato",
        "Sambag", "San Antonio", "Santo Niño", "Siocon", "Sudlonon", "Taytayan", "Tindog"
    ],
    "Alcantara": [
        "Cabadiangan", "Candabong", "Cansalonoy", "Lawaan", "Palanas", "Poblacion", "Polo", "Salagmaya"
    ],
    "Alcoy": [
        "Atabay", "Daan-Lungsod", "Guiwang", "Nug-as", "Poblacion", "Pugalo", "San Agustin"
    ],
    "Alegria": [
        "Compostela", "Guadalupe", "Lepanto", "Madridejos", "Montpeller", "Poblacion", "Santa Filomena", "Valencia"
    ],
    "Aloguinsan": [
        "Angilan", "Bojo", "Bonbon", "Calangcalan", "Candabong", "Esperanza", "Kandingan", "Kawasan", "Olango",
        "Poblacion", "Punay", "Rosario", "Saksak", "San Agustin", "San Isidro", "San Juan", "San Roque",
        "Santa Cruz", "Santa Filomena", "Sumaguan", "Taloot", "Tubod", "Zaragosa"
    ],
    "Argao": [
        "Alambijud", "Anajao", "Apo", "Balaas", "Balisong", "Binlod", "Bulasa", "Butong", "Calagasan", "Canbanua",
        "Capio-an", "Casay", "Conalum", "Cansuje", "Colawin", "Dagatan", "Guiwanon", "Gutlang", "Jampang",
        "Lamacan", "Langtad", "Lapay", "Linut-od", "Mompeller", "Poblacion", "Sua", "Talaytay", "Taloot", "Talaytay",
        "Tulang", "Usmad"
    ],
    "Asturias": [
        "Agtugop", "Bago", "Bag-o", "Bagongdan", "Baluarte", "Banban", "Baring", "Baye", "Biasong", "Bongol",
        "Cabcab", "Cambituan", "Campo", "Candugo", "Cogon", "Dapdap", "Duyan", "Estaca", "Guinohuan", "Kansi",
        "Langub", "Lanao", "Looc Norte", "Looc Sur", "Lupa", "Lutopan", "Manguiao", "Mantalongon", "New Bago",
        "Owak", "Poblacion", "Sak-sak", "San Isidro", "San Jose", "San Roque", "Santa Lucia", "Santa Rita",
        "Santo Niño", "Tag-amakan", "Tagbubunga", "Tagulaman", "Tan-awan", "Tubigagmanok", "Tubod"
    ],
    "Badian": [
        "Alawijao", "Balhaan", "Banago", "Basak", "Basiao", "Bato", "Bugas", "Calangcang", "Calangcang Sur",
        "Calangcang Norte", "Candabong", "Canlaob", "Catadman", "Dapdap", "Ginablan", "Lambug", "Looc", "Malhiao",
        "Malolos", "Manduyong", "Matutinao", "Patong", "Poblacion", "Sanlagan", "Santa Filomena", "Taguisa",
        "Talayong", "Zaragosa"
    ],
    "Balamban": [
        "Abucayan", "Aliwanay", "Arpili", "Atop-atop", "Buanoy", "Cabagdalan", "Cambuhawe", "Cambusay", "Campangga",
        "Cantibas", "Cantipla", "Cantuod", "Ginatilan", "Hingatmonan", "Lamesa", "Liki", "Lupo", "Luca", "Nangka",
        "Pondol", "Poblacion", "Prenza", "Pung-ol", "Saliring", "San Agustin", "San Isidro", "San Jose", "San Roque",
        "Santa Cruz", "Tan-awan", "Tinubdan", "Ubogon", "Vito"
    ],
    "Bantayan": [
        "Atop-atop", "Baigad", "Bakilid", "Balidbid", "Binaobao", "Botigues", "Buyo", "Cabangbang", "Cagban",
        "Campo", "Cangkao", "Daangtabogon", "Guintorilan", "Hagnaya", "Hilotongan", "Kabangbang", "Kampingganon",
        "Kaongkod", "Lipayran", "Lutao", "Mocaboc", "Mojon", "Nalundasan", "Ocoy", "Putian", "San Agustin",
        "Santa Cruz", "Sillon", "Sulangan", "Talisay", "Ticad"
    ],
    "Barili": [
        "Azucena", "Bagakay", "Balao", "Baliwagan", "Balog", "Bolocboloc", "Budbud", "Bugho", "Cadulawan",
        "Calambua", "Calunasan", "Candugay", "Dakit", "Guibuangan", "Guinsay", "Gunting", "Hilantagaan", "Japitan",
        "Lamak", "Lantawan", "Lepanto", "Maghanoy", "Malolos", "Mantalongon", "Nasipit", "Ocaña", "Panas",
        "Poblacion", "Punta", "Sayaw", "Tal-ot", "Tayasan", "Tubod"
    ],
    "Boljoon": [
        "Arbor", "Baclayan", "Balakbalak", "Blocon", "Bonbon", "Calngos", "Carunayan", "Dayhagon", "El Pardo",
        "Granada", "Japitan", "Lumbang", "Manloy", "Nangka", "Poblacion", "San Antonio", "San Isidro", "San Miguel",
        "San Roque", "San Vicente", "Tagbubunga"
    ],
    "Borbon": [
        "Bagongtabog", "Bagtic", "Bantigue", "Bayangan", "Bongbong", "Bontod", "Cabadiangan", "Cambungay", "Cangmangao",
        "Catangnan", "Clavera", "Cogon", "Daanlungsod", "Ginablan", "Guba", "Ilihan", "Laaw", "Log-ong", "Managase",
        "Poblacion", "Sagay", "San Isidro", "San Juan", "Santa Cruz", "Seta", "Tabay", "Tuburan", "Villahermosa"
    ],
    "Carmen": [
        "Baring", "Cogon", "Dawis Norte", "Dawis Sur", "Duang", "Garing", "Ipil", "Lower Natimao-an", "Luyang",
        "Magcagong", "Poblacion", "Sac-on", "San Isidro", "San Vicente", "Triumfo", "Upper Natimao-an"
    ],
    "Catmon": [
        "Agsuwao", "Amancion", "Anapog", "Anapog Bacay", "Basak", "Binongkalan", "Cabungahan", "Canao", "Catmondaan",
        "Corazon", "Duyan", "Ginablan", "Katipunan", "Lamintak Norte", "Lamintak Sur", "Macaas", "Poblacion",
        "Panalipan", "Santa Cruz", "Tabili", "Tinubdan"
    ],
    "Compostela": [
        "Basak", "Bawo", "Cabadiangan", "Cambayog", "Cogon", "Estaca", "Guindaruhan", "Lupa", "Magay", "Mulao",
        "Panangban", "Poblacion", "San Jose", "Tag-ubi", "Tamiao", "Tindog"
    ],
    "Consolacion": [
        "Cabangahan", "Cansaga", "Casili", "Danglag", "Garing", "Jugan", "Lamac", "Lanipga", "Nangka", "Pitogo",
        "Poblacion Occidental", "Poblacion Oriental", "Polog", "Pulpogan", "Sacsac", "San Roque", "Talamban",
        "Tugbongan", "Tulo-tulo", "Tayud"
    ],
    "Cordova": [
        "Alegria", "Bangbang", "Buagsong", "Catarman", "Cogon", "Dapitan", "Day-as", "Gabi", "Gilutongan", "Ibabao",
        "Pilipog", "Poblacion", "San Miguel"
    ],
    "Daanbantayan": [
        "Agujo", "Bagay", "Bakhawan", "Bateria", "Bitoon", "Calape", "Carnaza", "Dalingding", "Dalingding Sur",
        "Dalingding Norte", "Hilotongan", "Logon", "Malbago", "Poblacion", "Pajo", "Agujo", "Maya", "Paypay", "Suba",
        "Tapilon", "Tominjao", "Aguho", "Marikaban", "Palau", "Kawit"
    ],
    "Dalaguete": [
        "Ablayan", "Balud", "Balut", "Babayongan", "Bagacay", "Balawag", "Basak", "Binalabanan", "Bogo", "Boljoon",
        "Bongbong", "Cabadiangan", "Caliongan", "Cambaguio", "Camboang", "Camburoy", "Candelaria", "Consolacion",
        "Coro", "Dugyan", "Dumalan", "Dumanjug", "Guiwanon", "Igbarawan", "Kamunda", "Lanao", "Laya", "Langosig",
        "Luan-luan", "Malore", "Maluanluan", "Malucos", "Mangcalinao", "Manuyog", "Manuyog", "Maomao", "Maravilla",
        "Mayana", "Mompeller", "Montpeller", "Obong", "Pancil", "Paniguizon", "Poblacion", "Salug", "Sandayong",
        "Santo Niño", "Talamban", "Tangbo", "Taytayan"
    ],
    "Dumanjug": [
        "Balaygtiki", "Bitoon", "Bullas", "Candabong", "Candulawan", "Cansalonoy", "Cogon", "Damolog", "Kabalaasnan",
        "Kabawan", "Kambagtit", "Kamundang", "Kolabton", "Lambusan", "Mahanlud", "Mantayupan", "Matayupan", "Ocaña",
        "Palanas", "Poblacion", "San Isidro", "Santa Cruz", "Tangil", "Tapon"
    ],
    "Ginatilan": [
        "Ballud", "Ballud Sur", "Bojo", "Cambang-ug", "Campangga", "Canbang-ug", "Cantibas", "Capualan", "Cansayum",
        "Cassiloan", "Ilihan", "Lalan", "Looc", "Looc Norte", "Looc Sur", "Looc Tabo", "Malabuyoc", "Malagum",
        "Malapoc", "Mamba", "Mantayupan", "Maspati", "Milagros", "Obo", "Panugnawan", "Pasil", "Poblacion", "San Jose",
        "Santol", "Tagbubunga", "Tigbao", "Tigbao Sur", "Tigbao Norte", "Tigbao Proper", "Tigbao Silangan",
        "Tigbao Bayog", "Tigbao Silong", "Tigbao Ilihan", "Tigbao Santa Cruz", "Tigbao Villahermosa", "Tigbao Pitogo",
        "Tigbao Pilar"
    ],
    "Liloan": [
        "Cabadiangan", "Calero", "Catarman", "Cotcot", "Jubay", "Lataban", "Mulao", "Poblacion", "San Roque",
        "Santa Cruz", "Tayud", "Tugbongan", "Yati"
    ],
    "Madridejos": [
        "Bunacan", "Kaongkod", "Kodia", "Looc", "Malbago", "Pili", "Poblacion", "San Agustin", "Tabagak",
        "Talangnan", "Tarong"
    ],
    "Malabuyoc": [
        "Armeña", "Bala", "Calatagan", "Calongcalong", "Cerdeña", "Looc", "Mahanlud", "Mindanao", "Montañeza",
        "Poblacion I", "Poblacion II", "Santo Niño", "Sorsogon", "Tolosa"
    ],
    "Medellin": [
        "Antipolo", "Curva", "Daanlungsod", "Dayhagon", "Don Virgilio Gonzalez (formerly Luy-a)", "Gibitngil",
        "Kawit", "Lamintak Norte", "Lamintak Sur", "Luyongbaybay", "Mahawak", "Poblacion", "Tindog", "Tominhao"
    ],
    "Minglanilla": [
        "Cadulawan", "Calajoan", "Camp 7", "Camp 8", "Campo Siete", "Cuanos", "Guindaruhan", "Linao", "Lipata",
        "Manduang", "Pakigne", "Poblacion Ward I", "Poblacion Ward II", "Poblacion Ward III", "Poblacion Ward IV",
        "Poblacion Ward V", "Poblacion Ward VI", "Poblacion Ward VII", "Poblacion Ward VIII", "Tungkop", "Tulay",
        "Tunghaan", "Tubod", "Tungkil", "Vito", "Ward I", "Ward II", "Ward III", "Ward IV", "Ward V", "Ward VI",
        "Ward VII", "Ward VIII"
    ],
    "Moalboal": [
        "Agbalanga", "Bala", "Balabagon", "Basdiot", "Bugho", "Busay", "Batadbatad", "Bato", "Cansaga", "Cantabog",
        "Dolho", "Esperanza", "Gawi", "Ginatilan", "Lamac", "Malolos", "Mindanao", "Poblacion East", "Poblacion West",
        "Saavedra", "Tomonoy", "Tuble"
    ],
    "Oslob": [
        "Alo", "Aguio", "Balagunan", "Bangcogon", "Bonbon", "Calumpang", "Canang", "Cansaloay", "Cantawan", "Catadman",
        "Daanlungsod", "Gawi", "Ginatilan", "Lagundi", "Liloan", "Looc", "Mag-Ato", "Nueva Caceres", "Poblacion",
        "Poblacion II", "Tan-awan", "Tubod"
    ],
    "Pilar": [
        "Biasong", "Buenavista", "Cawit", "Consolacion", "Dapdap", "Esperanza", "Lanao", "Magsaysay", "Moabog",
        "Poblacion", "San Isidro", "San Juan", "Santa Rosa"
    ],
    "Pinamungajan": [
        "Anislag", "Anopog", "Binabag", "Buhingtubig", "Busay", "Butong", "Camugao", "Duangan", "Guimbawian",
        "Lamac", "Lut-od", "Pandacan", "Poblacion", "Sibago", "Tajao", "Tangub", "Tupas"
    ],
    "Poro": [
        "Adela", "Altavista", "Cagcagan", "Campamanog", "Cansabusab", "Cansaga", "Daan Paz", "Eastern Poblacion",
        "Libertad", "Mac", "Mabini", "Mercedes", "Rizal", "Santa Rita", "Teguis", "Western Poblacion"
    ],
    "Ronda": [
        "Butong", "Canduling", "Cansalonoy", "Liboo", "Malalay", "Nasipit", "Poblacion", "Santa Cruz", "Tupas"
    ],
    "Samboan": [
        "Basak", "Bonbon", "Bulangsuran", "Calatagan", "Camburoy", "Can-ukban", "Colase", "Dalahican", "Gawi",
        "Mainit", "Monteverde", "Poblacion", "San Sebastian", "Santa Cruz", "Tapon", "Tangbo"
    ],
    "San Fernando": [
        "Balud", "Balungag", "Basak", "Bugho", "Cabatbatan", "Greenhills", "Ilaya", "Lantawan", "Liburon", "Magsico",
        "North Poblacion", "Panadtaran", "South Poblacion", "San Isidro", "Sangat", "Tabionan", "Tañañas", "Tinubdan"
    ],
    "San Francisco": [
        "Cabunga-an", "Campo", "Consuelo", "Esperanza", "Himensulan", "Santiago", "Union", "Western Poblacion"
    ],
    "San Remigio": [
        "Anapog", "Argawanon", "Bancasan", "Bogo", "Busogon", "Calambua", "Cambunag", "Canagahan", "Cantipay",
        "Dapdap", "Gawaygaway", "Hagnaya", "Kangyano", "Lambusan", "Lawis", "Libaong", "Looc", "Luyang", "Maño",
        "Poblacion", "Sab-a", "San Miguel", "Tacup", "Tambongon", "Toong", "Victoria"
    ],
    "Santa Fe": [
        "Balidbid", "Hagdan", "Hilantagaan", "Langub", "Maricaban", "Okoy", "Poblacion", "Pooc", "Talisay"
    ],
    "Santander": [
        "Bunlan", "Candamiang", "Canlumacad", "Liloan", "Lip-tong", "Looc", "Pasil", "Poblacion", "Talisay"
    ],
    "Sibonga": [
        "Abugon", "Bahay", "Bae", "Bagacay", "Bahay", "Basak", "Bato", "Bonga", "Bulasa", "Cabadiangan", "Cambigong",
        "Can-aga", "Candaguit", "Candangcalan", "Cantao-an", "Catang", "Colase", "Damolog", "Dapdap", "Doldol",
        "Inayagan", "Japitan", "Lamacan", "Libo", "Lindogon", "Lumbang", "Magcagong", "Mag-aso", "Manatad",
        "Mayana", "Montealegre", "Poblacion", "Puli", "Sabang", "San Isidro", "Sayao", "Talo-ot"
    ],
    "Sogod": [
        "Ampongol", "Bagatayam", "Bagacay", "Bawo", "Cabalawan", "Cabangahan", "Calumboyan", "Damolog", "Ibabao",
        "Lamac", "Liki", "Lubo", "Mohon", "Nahus-an", "Pamutan", "Pansoy", "Pandan", "Paril", "Patag", "Poblacion",
        "Tabunok", "Tagnucan"
    ],
    "Tabogon": [
        "Anonang Norte", "Anonang Sur", "Bagatayam", "Bawo", "Bongon", "Cabangahan", "Canaocanao", "Daan Tapul",
        "Kal-anan", "Kawit", "Labangon", "Loong", "Mabuli", "Managase", "Maslog", "Mompeller", "Pio", "Poblacion",
        "Sambag", "San Isidro", "Santa Rosa", "Somosa", "Tapul", "Tugas"
    ],
    "Tabuelan": [
        "Bongon", "Budbud", "Dalid", "Kantubaon", "Mabunao", "Maravilla", "Olivo", "Poblacion", "Tabunok",
        "Tigbawan"
    ],
    "Tuburan": [
        "Alegria", "Amatugan", "Antipolo", "Bagasawe", "Bagong Sirang", "Bulwang", "Carmelo", "Colonia", "Curva",
        "Dawis", "Fortaliza", "Kabangkalan", "Kalangahan", "Kampoot", "Kansumoroy", "Kanlunsing", "Koob", "Lanao",
        "Lebriohan", "Liburon", "Mag-antoy", "Montealegre", "Patag", "Poblacion", "Putat", "San Isidro", "San Juan",
        "San Roque", "San Vicente", "Sumon", "Tabunok", "Tominjao"
    ],
    "Tudela": [
        "Basilia", "Bukil", "Calangcang", "Carmen", "Daan Lungsod", "General", "Lanao", "Magsaysay", "Mc Arthur",
        "Northern Poblacion", "Puertobello", "San Isidro", "San Vicente", "Santander", "Santa Maria",
        "Southern Poblacion"
    ]
};

$(document).ready(function () {
    $('#city').change(function () {
        const city = $(this).val();
        const barangaySelect = $('#barangay');
        barangaySelect.empty();
        barangaySelect.append('<option value="" disabled selected>Barangay</option>');

        if (city && barangays[city]) {
            barangays[city].forEach(function (barangay) {
                barangaySelect.append(`<option value="${barangay}">${barangay}</option>`);
            });
        }
    });

    $('form').on('submit', function () {
        var city = $('select[name="city"]').val();
        var barangay = $('select[name="barangay"]').val();
        var zipcode = $('input[name="zipcode"]').val();
        var address = city + ', ' + barangay + ', ' + zipcode;
        $('<input>').attr({
            type: 'hidden',
            name: 'address',
            value: address
        }).appendTo('form');
    });
});