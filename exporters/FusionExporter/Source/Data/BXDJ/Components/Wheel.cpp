#include "Wheel.h"
#include "../RigidNode.h"
#include "../Joints/RotationalJoint.h"
#include "../../BXDA/Mesh.h"

using namespace BXDJ;

Wheel::Wheel(const Wheel & wheelToCopy)
{
	type = wheelToCopy.type;
	frictionLevel = wheelToCopy.frictionLevel;
	isDriveWheel = wheelToCopy.isDriveWheel;
	radius = wheelToCopy.radius;
	width = wheelToCopy.width;
	center = wheelToCopy.center;
}

Wheel::Wheel(Type type, FrictionLevel frictionLevel, bool isDriveWheel)
{
	this->type = type;
	this->frictionLevel = frictionLevel;
	this->isDriveWheel = isDriveWheel;
	radius = 0;
	width = 0;
	center = Vector3<>();
}

Wheel::Wheel(const Wheel & wheel, const RotationalJoint & joint) : Wheel(wheel)
{
	Vector3<> axis = joint.getAxisOfRotation();
	center = joint.getChildBasePoint();

	// Calculate radius and width
	BXDA::Mesh mesh(joint.getChild()->getGUID());
	joint.getChild()->getMesh(mesh, true);

	double minWidth, maxWidth;
	mesh.calculateWheelShape(axis, center, minWidth, maxWidth, radius);

	width = maxWidth - minWidth;
	center = center + axis * (width / 2); // Offset center to actual center of wheel
}

double Wheel::getRadius() const
{
	return radius;
}

double Wheel::getWidth() const
{
	return width;
}

Vector3<> Wheel::getCenter() const
{
	return center;
}

#pragma region FrictionInfo

float BXDJ::Wheel::getForwardAsympSlip() const
{
	return 1.5f;
}

float BXDJ::Wheel::getForwardAsympValue() const
{
	switch (frictionLevel)
	{
	case LOW: return 3.0f;
	case MEDIUM: return 5.0f;
	case HIGH: return 8.0f;
	}
	return 0.0f;
}

float BXDJ::Wheel::getForwardExtremeSlip() const
{
	return 1.0f;
}

float BXDJ::Wheel::getForwardExtremeValue() const
{
	switch (frictionLevel)
	{
	case LOW: return 5.0f;
	case MEDIUM: return 7.0f;
	case HIGH: return 10.0f;
	}
	return 0.0f;
}

float BXDJ::Wheel::getSideAsympSlip() const
{
	return 1.5f;
}

float BXDJ::Wheel::getSideAsympValue() const
{
	if (type == OMNI)
		return 0.005f;
	else
		switch (frictionLevel)
		{
		case LOW: return 3.0f;
		case MEDIUM: return 5.0f;
		case HIGH: return 8.0f;
		}
	return 0.0f;
}

float BXDJ::Wheel::getSideExtremeSlip() const
{
	return 1.0f;
}

float BXDJ::Wheel::getSideExtremeValue() const
{
	if (type == OMNI)
		return 0.01f;
	else
		switch (frictionLevel)
		{
		case LOW: return 5.0f;
		case MEDIUM: return 7.0f;
		case HIGH: return 10.0f;
		}
	return 0.0f;
}

#pragma endregion

rapidjson::Value Wheel::getJSONObject(rapidjson::MemoryPoolAllocator<>& allocator) const
{
	rapidjson::Value wheelJSON;
	wheelJSON.SetObject();

	wheelJSON.AddMember("type", rapidjson::Value((int)type), allocator);
	wheelJSON.AddMember("frictionLevel", rapidjson::Value((int)frictionLevel), allocator);
	wheelJSON.AddMember("isDriveWheel", rapidjson::Value(isDriveWheel), allocator);

	return wheelJSON;
}

void Wheel::loadJSONObject(const rapidjson::Value & wheelJSON)
{
	if (wheelJSON.IsObject())
	{
		if (wheelJSON["type"].IsNumber())
			type = (Wheel::Type)wheelJSON["type"].GetInt();
		if (wheelJSON["frictionLevel"].IsNumber())
			frictionLevel = (Wheel::FrictionLevel)wheelJSON["frictionLevel"].GetInt();
		if (wheelJSON["isDriveWheel"].IsBool())
			isDriveWheel = wheelJSON["isDriveWheel"].GetBool();
	}
}

void Wheel::write(XmlWriter & output) const
{
	output.startElement("WheelDriverMeta");
	output.writeAttribute("DriverMetaID", "0");

	output.writeElement("WheelType", toString(type));
	output.writeElement("WheelRadius", std::to_string(getRadius()));
	output.writeElement("WheelWidth", std::to_string(getWidth()));

	output.startElement("BXDVector3");
	output.writeAttribute("VectorID", "WheelCenter");
	output.write(getCenter());
	output.endElement();

	// Friction Info
	output.writeElement("ForwardAsympSlip", std::to_string(getForwardAsympSlip()));
	output.writeElement("ForwardAsympValue", std::to_string(getForwardAsympValue()));
	output.writeElement("ForwardExtremeSlip", std::to_string(getForwardExtremeSlip()));
	output.writeElement("ForwardExtremeValue", std::to_string(getForwardExtremeValue()));
	output.writeElement("SideAsympSlip", std::to_string(getSideAsympSlip()));
	output.writeElement("SideAsympValue", std::to_string(getSideAsympValue()));
	output.writeElement("SideExtremeSlip", std::to_string(getSideExtremeSlip()));
	output.writeElement("SideExtremeValue", std::to_string(getSideExtremeValue()));

	output.writeElement("IsDriveWheel", isDriveWheel ? "true" : "false");

	output.endElement();
}

std::string Wheel::toString(Type type)
{
	switch (type)
	{
	case NORMAL:  return "NORMAL";
	case OMNI:    return "OMNI";
	case MECANUM: return "MECANUM";
	}

	return "UNKNOWN";
}
