open util/boolean

abstract sig EventType { }

lone sig LUNCH extends EventType { }
lone sig WORK extends EventType { }
lone sig HOME extends EventType { } 
lone sig BIRTHDAY extends EventType { }
lone sig HOLIDAY extends EventType { }

abstract sig TravelMeanType { }

lone sig CAR extends TravelMeanType { }
lone sig WALKING extends TravelMeanType { }
lone sig BICYCLE extends TravelMeanType { }
lone sig PUBLIC_TRANSPORT extends TravelMeanType { }

abstract sig AccidentType { }

lone sig STRIKE extends AccidentType { }
lone sig BAD_WEATHER extends AccidentType { }
lone sig OUT_OF_SERVICE extends AccidentType{}


sig Float { }
lone sig Stringa{}

sig Date{
	day: one Int,
	time: one Int
}

sig Position {
	latitude: one Int, 
	longitude: one Int
} 

one sig User{
	userName: one Stringa,
//	name: one Stringa,
//	surnmae: one Stringa,
	email: one Stringa,
//	phoneNumber: one Int,
	actualPosition: one Position,
	calendar: one Calendar
}{
	
}

sig Calendar{
	user: one User,
	event: some Event
}{
}

sig Transportation {
	travelMean: one TravelMeanType,
	accidentType: lone AccidentType,
	isShared: one Bool,
	isAccident: one Bool
}{
	(travelMean = PUBLIC_TRANSPORT or travelMean = WALKING) implies (isShared = False)
	(one accidentType) implies (isAccident = True)
}

sig Location {
	location: lone Position,
	transport: lone Transportation,
	isReachable: one Bool
}{
	/* GPS off? */
	(location.latitude =  0 and location.longitude = 0) implies (no
	transport and isReachable = False) else (one transport and isReachable = True)
	
	(no transport) implies isReachable = False	
}

sig Event {
//	eventName: lone String,
//	currentPosition: one Position,
	destination: one Location,
	startDate: one Date,
	endDate: one Date,
	eventType: lone EventType,
	recurrence: lone Int,
	calendar: one Calendar,
	isAllDay: one Bool
}{
	startDate.day <= endDate.day
	
	(eventType = LUNCH) implies (startDate.day = endDate.day and isAllDay = False)

	(isAllDay = True) implies (startDate = endDate)
}

//Association 1-1 between Calendar and User
fact associationCalendarUser {
	all c:Calendar, u:User |
		(c in u.calendar <=> (u in c.user))
}

/*Assiociation 1-1 between Calendar and Event*/
fact associationCalendarEvent {
	all c:Calendar, e:Event |
		(c in e.calendar <=> (e in c.event))
}

/* No multiple users with same email or username  */
fact userUnivocity {
	no u1, u2: User | (u1 != u2) and (u1.userName = u2.userName and u1.email = u2.email)
}

/*Different calendar*/
fact calendarUnivocity{
	no c1,c2: Calendar | (c1 != c2) and (c1.user = c2.user)
}

/*An event that starts and ends in the same day, must has startTime lower than endTime*/
fact{
	all e:Event | (e.startDate.day = e.endDate.day and e.isAllDay = False)
		implies
		e.startDate.time < e.endDate.time
}

/*An event that starts and ends in the different days, can have any startTime or endTime*/
fact{
	all e:Event |e.startDate.day != e.endDate.day
		implies (one e.startDate.time and one e.endDate.time)
}

/* Two events must not overlap */
fact noEventAtSameTime {
	all e1,e2: Event | (e1 != e2) and (e1.startDate.day + e1.endDate.day = e2.startDate.day + e2.endDate.day)
		implies
		(e1.endDate.time < e2.startDate.time or e2.endDate.time < e1.startDate.time)
}

/* There must be one event corresponding to lunch per day */
fact oneLunchPerDay {
	no e1, e2: Event | e1 != e2 and e1.eventType = LUNCH and e2.eventType = LUNCH and e1.startDate.day + e1.endDate.day= e2.startDate.day + e2.endDate.day
}

/*If the application recognizes a bad weather. travel means like walking and bicycle are not available*/
fact {
	all t: Transportation | t.accidentType = BAD_WEATHER implies (t.travelMean != WALKING and t.travelMean != BICYCLE)
}

/*Public Transport is not a solution if there is a strike or the transportation are out of service*/
fact {
	all t: Transportation | t.accidentType = STRIKE or  t.accidentType = OUT_OF_SERVICE  implies (t.travelMean != PUBLIC_TRANSPORT)
}

/*Every Location must have at least one event associated*/
fact{
	all l: Location | some e: Event | l in e.destination
}

/*Every location that does not have transport must be unreachable */
fact{
	all l: Location | #l.transport = 0
		implies
		l.isReachable = False
}

/*			ASSERTION			*/

assert noEventOverlapping {
	all e1,e2: Event | (e1 != e2) and (e1.startDate.day + e1.endDate.day = e2.startDate.day + e2.endDate.day)
		implies
		(e1.endDate.time < e2.startDate.time or e2.endDate.time < e1.startDate.time)
}

//check noEventOverlapping 
//OK

assert noEventWithoutCaldendar{
	all e: Event | #e.calendar = 1
}

//check noEventWithoutCaldendar
//OK

assert allDayEvents{
	no e:Event | e.isAllDay = True and ( e.startDate != e.endDate)
}

//check allDayEvents
//OK

assert noEventWithEndDateLowerThanStartDate{
	no e: Event | e.startDate.day = e.endDate.day and e.isAllDay = False and (e.startDate.time > e.endDate.time)
}

//check noEventWithEndDateLowerThanStartDate
//OK

assert noEventWithEndDateBeforeStartDate{
	no e: Event | e.startDate.day < e.startDate.day
}

//check noEventWithEndDateBeforeStartDate
//OK

assert noLocationUnreachableWithTransportAssociated{
	no l: Location | l.isReachable = False and #l.transport =  1
}

//check noLocationUnreachableWithTransportAssociated
//OK

assert prova{
	no t: Transportation | t.isShared = True and t.travelMean = WALKING
}

check prova


/* 	PREDICATES			*/

pred show(){
	#Location >= 1
	#Event >= 1
	#Position >= 1
	#Transportation >= 1
	#{t: Transportation | t.isShared = True} = 1
	#{e: Event | e.eventType = LUNCH} = 3}

run show for 10



